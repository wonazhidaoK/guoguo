using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 微信相关
    /// </summary>
    public class WXController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeiXinUserRepository _weiXinUserRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IVipOwnerCertificationRecordRepository _vipOwnerCertificationRecordRepository;
        private readonly IVipOwnerApplicationRecordRepository _vipOwnerApplicationRecordRepository;
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="weiXinUserRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="vipOwnerCertificationRecordRepository"></param>
        /// <param name="vipOwnerApplicationRecordRepository"></param>
        /// <param name="vipOwnerRepository"></param>
        public WXController(IUserRepository userRepository,
            IWeiXinUserRepository weiXinUserRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository,
            IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository,
            IVipOwnerRepository vipOwnerRepository)
        {
            _userRepository = userRepository;
            _weiXinUserRepository = weiXinUserRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _vipOwnerCertificationRecordRepository = vipOwnerCertificationRecordRepository;
            _vipOwnerApplicationRecordRepository = vipOwnerApplicationRecordRepository;
            _vipOwnerRepository = vipOwnerRepository;
            _tokenManager = new TokenManager();
        }

        #region 微信服务器消息接收及处理

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [Route("WeiXin")]
        public HttpResponseMessage Get(string signature, string timestamp, string nonce, string echostr)
        {
            //BackgroundJob.Enqueue(() => SendAsync("888"));
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                var result = new StringContent(echostr, UTF8Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = new HttpResponseMessage { Content = result };
                return response;
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [Route("WeiXin")]
        public async Task<HttpResponseMessage> Post(CancellationToken cancelToken)
        {
            var requestQueryPairs = Request.GetQueryNameValuePairs().ToDictionary(k => k.Key, v => v.Value);
            if (requestQueryPairs.Count == 0
                || !requestQueryPairs.ContainsKey("timestamp")
                || !requestQueryPairs.ContainsKey("signature")
                || !requestQueryPairs.ContainsKey("nonce")
                || !CheckSignature.Check(requestQueryPairs["signature"], requestQueryPairs["timestamp"],
                    requestQueryPairs["nonce"], Token))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "未授权请求");
            }
            PostModel postModel = new PostModel
            {
                Signature = requestQueryPairs["signature"],
                Timestamp = requestQueryPairs["timestamp"],
                Nonce = requestQueryPairs["nonce"]
            };
            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;
            var ss = Request.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(ss, Encoding.GetEncoding("utf-8"));
            var json = reader.ReadToEnd();
            //BackgroundJob.Enqueue(() => SendAsync(json));
            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new WXCustomMessageHandler(Request.Content.ReadAsStreamAsync().Result, postModel, maxRecordCount);

            try
            {
                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                await messageHandler.ExecuteAsync(cancelToken);

                var resMessage = Request.CreateResponse(HttpStatusCode.OK);

                string content = messageHandler.ResponseDocument == null ? string.Empty : messageHandler.ResponseDocument.ToString();

                resMessage.Content = new StringContent(content);

                resMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

                return resMessage;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error("处理微信请求出错：", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "处理微信请求出错，内容：" + ex.Message);
            }
        }

        #endregion

        #region 人机交互



        /// <summary>
        /// 员工扫描注册二维码后，向员工发送注册提醒
        /// </summary>
        public void SendEmployeeRegisterRemind()
        {
            try
            {
                //var accessToken = AccessTokenContainer.GetAccessToken(AppId);
                //更换成你需要的模板消息ID
                string templateId = "AXA-AqlSepXjKzSldchlUXUFtCaVE9cJaX4pMkuhJ-I";//ConfigurationManager.AppSettings["WXTemplate_EmployeeRegisterRemind"].ToString();
                                                                                  //更换成对应的模板消息格式
                var templateData = new
                {
                    first = new TemplateDataItem("用户认证通知"),
                    //  account = new TemplateDataItem(wxNickName),
                    keyword1 = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                    keyword2 = new TemplateDataItem("888"),
                    keyword3 = new TemplateDataItem(""),
                    remark = new TemplateDataItem("详情", "#FF0000")
                };

                var miniProgram = new TempleteModel_MiniProgram()
                {
                    appid = GuoGuoCommunity_WxOpenAppId,//ZhiShiHuLian_WxOpenAppId,
                                                        //pagepath = "pages/editmyinfo/editmyinfo?id=" + employeeID
                };

                TemplateApi.SendTemplateMessage(AppId, "oTK8q0-mRSd44GbjJeknfz0vLv6I", templateId, null, templateData, miniProgram);
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }

        }


        #endregion

        #region 获取手机号
        /// <summary>
        /// 获取用户手机号(根据逻辑自己改造)
        /// </summary>
        /// <param name="jsonObj">
        ///     Code    小程序授权码
        ///     EncryptedData
        ///     IV
        /// </param>
        /// <returns></returns>
        [Route("GetUserPhone")]
        [HttpPost]
        [AllowAnonymous]
        public ApiResult GetUserPhone([FromBody]dynamic jsonObj)
        {
            string code = jsonObj.Code;
            string encryptedData = jsonObj.EncryptedData;
            string iv = jsonObj.IV;

            ApiResult apiResult = new ApiResult();

            var openIdResult = SnsApi.JsCode2Json(GuoGuoCommunity_WxOpenAppId, GuoGuoCommunity_WxOpenAppSecret, code);
            //openIdResult.unionid
            //   openIdResult.openid
            //DecodedPhoneNumber decodedPhoneNumber = EncryptHelper.DecryptPhoneNumber(openIdResult.session_key, encryptedData, iv);

            string phoneJson = EncryptHelper.DecodeEncryptedData(openIdResult.session_key, encryptedData, iv);

            return null;
        }
        #endregion

        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("GetOpenId")]
        [HttpGet]
        public ApiResult<string> GetOpenId([FromUri]string code)
        {
            var openIdResult = SnsApi.JsCode2Json(GuoGuoCommunity_WxOpenAppId, GuoGuoCommunity_WxOpenAppSecret, code);

            return new ApiResult<string>(APIResultCode.Success, openIdResult.openid);
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        [Route("wx/Login")]
        [HttpGet]
        public async Task<ApiResult<WXLoginOutput>> Login([FromUri]string code, CancellationToken cancelToken)
        {
            try
            {
                var openIdResult = SnsApi.JsCode2Json(GuoGuoCommunity_WxOpenAppId, GuoGuoCommunity_WxOpenAppSecret, code);
                var user = await _userRepository.GetForOpenIdAsync(new UserDto { OpenId = openIdResult.openid });

                if (user == null)
                {
                    user = await _userRepository.AddWeiXinAsync(new UserDto() { OpenId = openIdResult.openid, UnionId = openIdResult.unionid }, cancelToken);
                }
                //产生 Token
                var token = _tokenManager.Create(user);

                //存入数据库
                await _userRepository.UpdateTokenAsync(
                    new UserDto
                    {
                        Id = user.Id.ToString(),
                        RefreshToken = token.Refresh_token
                    });
                var weiXinUser = await _weiXinUserRepository.GetAsync(openIdResult.unionid, cancelToken);
                /*
                 * 一期只有一个认证
                 */
                var ownerCertificationList = await _ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto() { UserId = user.Id.ToString() });
                var isVipOwner = false;
                if (ownerCertificationList.Any())
                {
                    var vipOwner = await _vipOwnerRepository.GetForSmallDistrictIdAsync(new VipOwnerDto { SmallDistrictId = ownerCertificationList.FirstOrDefault().SmallDistrictId });
                    if (vipOwner != null)
                    {
                        var vipOwnerCertificationRecord = await _vipOwnerCertificationRecordRepository.GetForVipOwnerIdAsync(new VipOwnerCertificationRecordDto
                        {
                            VipOwnerId = vipOwner.Id.ToString(),
                            UserId = user.Id.ToString()
                        });
                        if (vipOwnerCertificationRecord != null)
                        {
                            isVipOwner = true;
                        }
                    }
                }
                return new ApiResult<WXLoginOutput>(APIResultCode.Success, new WXLoginOutput()
                {
                    OpenId = user.OpenId,
                    Token = token.Access_token,
                    Headimgurl = weiXinUser?.Headimgurl,
                    Nickname = weiXinUser?.Nickname,
                    IsSubscription = weiXinUser == null ? false : true,
                    IsOwner = ownerCertificationList.Any(),
                    IsVipOwner = isVipOwner
                }, APIResultMessage.Success);

            }
            catch (Exception e)
            {
                return new ApiResult<WXLoginOutput>(APIResultCode.Success_NoB, new WXLoginOutput() { }, e.Message);
            }
        }
    }
}
