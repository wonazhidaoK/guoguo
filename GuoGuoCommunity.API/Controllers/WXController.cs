﻿using GuoGuoCommunity.API.Common;
using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using Newtonsoft.Json;
using NLog;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.TenPay.V3;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Helpers;
using System;
using System.Configuration;
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
        private static readonly Logger AppLogger = LogManager.GetCurrentClassLogger();
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        private readonly IUserRepository _userRepository;
        private readonly IWeiXinUserRepository _weiXinUserRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IVipOwnerCertificationRecordRepository _vipOwnerCertificationRecordRepository;
        private readonly IVipOwnerApplicationRecordRepository _vipOwnerApplicationRecordRepository;
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// 
        /// </summary>
        public WXController(IUserRepository userRepository,
            IWeiXinUserRepository weiXinUserRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository,
            IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository,
            IVipOwnerRepository vipOwnerRepository,
            ITokenRepository tokenRepository,
            IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _weiXinUserRepository = weiXinUserRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _vipOwnerCertificationRecordRepository = vipOwnerCertificationRecordRepository;
            _vipOwnerApplicationRecordRepository = vipOwnerApplicationRecordRepository;
            _vipOwnerRepository = vipOwnerRepository;
            _tokenRepository = tokenRepository;
            _orderRepository = orderRepository;
        }

        #region 微信服务器消息接收及处理

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [Route("WeiXin")]
        public HttpResponseMessage Get(string signature, string timestamp, string nonce, string echostr)
        {
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "处理微信请求出错，内容：" + ex.Message);
            }
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
                    user = await _userRepository.AddWeiXinAsync(new UserDto()
                    {
                        OpenId = openIdResult.openid,
                        UnionId = openIdResult.unionid,
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = "system"
                    }, cancelToken);
                }
                //产生 Token
                var token = _tokenRepository.Create(user);

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
                //var isVipOwner = false;
                //if (ownerCertificationList.Any())
                //{
                //    var vipOwner = await _vipOwnerRepository.GetForSmallDistrictIdAsync(new VipOwnerDto { SmallDistrictId = ownerCertificationList.FirstOrDefault().Industry.BuildingUnit.Building.SmallDistrictId.ToString() });
                //    if (vipOwner != null)
                //    {
                //        var vipOwnerCertificationRecord = await _vipOwnerCertificationRecordRepository.GetForVipOwnerIdAsync(new VipOwnerCertificationRecordDto
                //        {
                //            VipOwnerId = vipOwner.Id.ToString(),
                //            UserId = user.Id.ToString()
                //        });
                //        if (vipOwnerCertificationRecord != null)
                //        {
                //            isVipOwner = true;
                //        }
                //    }
                //}
                return new ApiResult<WXLoginOutput>(APIResultCode.Success, new WXLoginOutput()
                {
                    OpenId = user.OpenId,
                    Token = token.Access_token,
                    Headimgurl = weiXinUser?.Headimgurl,
                    Nickname = weiXinUser?.Nickname,
                    IsSubscription = weiXinUser == null ? false : true,
                    IsOwner = ownerCertificationList.Any(),
                    //IsVipOwner = isVipOwner
                }, APIResultMessage.Success);

            }
            catch (Exception e)
            {
                return new ApiResult<WXLoginOutput>(APIResultCode.Success_NoB, new WXLoginOutput() { }, e.Message);
            }
        }


        #region 支付

        #region 获取客户端请求的IP地址

        /// <summary>
        /// 获取客户端地址
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>IP地址</returns>
        private string GetClientIpAddress(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }
 
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic owinContext = request.Properties[OwinContext];
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }

            return null;
        }

        #endregion


        /// <summary>
        /// 创建订单成功后调起支付
        /// </summary>
        ///     json包
        ///             Code        微信请求时发送的Code
        ///             Price       微信请求时发送的价格
        /// <returns></returns>
        [Route("weixin/pay")]
        [HttpPost]
        public async Task<ApiResult<WeixinJsPaySignature>> Pay([FromBody]PayModelInput model, CancellationToken cancelToken)
        {
            /*
             * 接口根据订单id和小程序Code
             * 获取支付调起参数
             */
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                throw new NotImplementedException("订单Id信息为空！");
            }

            //查询订单
            var data = await _orderRepository.GetIncludeAsync(model.Id, cancelToken);

            ApiResult<WeixinJsPaySignature> apiResult = new ApiResult<WeixinJsPaySignature>();
            _ = new ApiResult();

            string timeStamp = TenPayV3Util.GetTimestamp();
            AppLogger.Debug(JsonConvert.SerializeObject("1" + timeStamp), JsonConvert.SerializeObject(timeStamp));

            string nonceStr = TenPayV3Util.GetNoncestr();
            AppLogger.Debug(JsonConvert.SerializeObject("2" + nonceStr), JsonConvert.SerializeObject(nonceStr));

            string PayV3_TenpayNotify = ConfigurationManager.AppSettings["PayV3_TenpayNotify"];
            TenPayV3Info tenPayV3Info = new TenPayV3Info(GuoGuoCommunity_WxOpenAppId, GuoGuoCommunity_WxOpenAppSecret, PayV3_MchId, PayV3_Key, string.Empty, string.Empty, PayV3_TenpayNotify, string.Empty);
            AppLogger.Debug(JsonConvert.SerializeObject(tenPayV3Info), JsonConvert.SerializeObject(tenPayV3Info));

            var openIdResult = SnsApi.JsCode2Json(GuoGuoCommunity_WxOpenAppId, GuoGuoCommunity_WxOpenAppSecret, model.Code);
            AppLogger.Debug(JsonConvert.SerializeObject(openIdResult), JsonConvert.SerializeObject(openIdResult));

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(tenPayV3Info.AppId, tenPayV3Info.MchId, "呙呙社区购物", data.Number, Convert.ToInt32(data.PaymentPrice * 100), GetClientIpAddress(Request), tenPayV3Info.TenPayV3Notify, Senparc.Weixin.TenPay.TenPayV3Type.JSAPI, openIdResult.openid, tenPayV3Info.Key, nonceStr, attach: data.Id.ToString());

            AppLogger.Debug(DateTime.Now.ToString("yyyyMMddHHmmss") + "****TenPayV3UnifiedorderRequestData对象" + JsonConvert.SerializeObject(xmlDataInfo), "****TenPayV3UnifiedorderRequestData对象" + JsonConvert.SerializeObject(xmlDataInfo));

            var resultPay = await TenPayV3.UnifiedorderAsync(xmlDataInfo);
            AppLogger.Debug("****TenPayV3.Unifiedorder返回对象" + JsonConvert.SerializeObject(resultPay), "****TenPayV3.Unifiedorder返回对象" + JsonConvert.SerializeObject(resultPay));

            if (resultPay.return_code.ToUpper() == "SUCCESS")
            {
                if (resultPay.result_code.ToUpper() == "SUCCESS")
                {
                    //设置支付参数
                    RequestHandler paySignReqHandler = new RequestHandler(null);
                    paySignReqHandler.SetParameter("appId", tenPayV3Info.AppId);
                    paySignReqHandler.SetParameter("timeStamp", timeStamp);
                    paySignReqHandler.SetParameter("nonceStr", nonceStr);
                    paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", resultPay.prepay_id));
                    paySignReqHandler.SetParameter("signType", "MD5");
                    paySignReqHandler.SetParameter("nonceStr", nonceStr);
                    string paySign = paySignReqHandler.CreateMd5Sign("key", tenPayV3Info.Key);
                    var jsmodel = new WeixinJsPaySignature
                    {
                        AppId = tenPayV3Info.AppId,
                        Timestamp = timeStamp,
                        NonceStr = nonceStr,
                        Package = string.Format("prepay_id={0}", resultPay.prepay_id),
                        PaySign = paySign,
                        OrderId = data.Number,
                        SignType = "MD5"
                    };
                    apiResult.Data = jsmodel;
                    return apiResult;
                }
                else
                {

                }
            }
            else
            {
                throw new NotImplementedException(JsonConvert.SerializeObject(resultPay));
            }

            return apiResult;
        }

        /// <summary>
        /// 支付回调
        /// 对后台通知交互时，如果微信收到商户的应答不是成功或超时，微信认为通知失败，微信会通过一定的策略定期重新发起通知，尽可能提高通知的成功率，但微信不保证通知最终能成功。 （通知频率为15/15/30/180/1800/1800/1800/1800/3600，单位：秒）
        /// </summary>
        /// <returns></returns>
        [Route("weixin/payNotifyUrl")]
        [HttpPost]
        public async Task<HttpResponseMessage> PayNotifyUrl( CancellationToken cancelToken)
        {
            ZFResponseHandler resHandler = new ZFResponseHandler(Request);
            //AppLogger.Debug(DateTime.Now.ToString("yyyyMMddHHmmss") + "****Request" + JsonConvert.SerializeObject(Request), "****Request" + JsonConvert.SerializeObject(Request));
            AppLogger.Debug(DateTime.Now.ToString("yyyyMMddHHmmss") + "****resHandler" + JsonConvert.SerializeObject(resHandler), "****resHandler" + JsonConvert.SerializeObject(resHandler));
            string return_code = resHandler.GetParameter("return_code");
            _ = resHandler.GetParameter("return_msg");
            //VIPState vip = _icustomer.GetVIPStateByOrderCode("sss");
            //await VIPCreateResult(vip.CreateDate, vip.VIPEndDate, "ddd");

            string attach = resHandler.GetParameter("attach");
            AppLogger.Debug(DateTime.Now.ToString("yyyyMMddHHmmss") + "****attach" + JsonConvert.SerializeObject(attach), "****attach" + JsonConvert.SerializeObject(attach));
            if (string.IsNullOrWhiteSpace(attach))
            {
                return new HttpResponseMessage { Content = new StringContent("", Encoding.GetEncoding("UTF-8"), "text/plain") };
            }
            resHandler.SetKey(PayV3_Key);


            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
            {
                //正确的订单处理
                string result_code = resHandler.GetParameter("result_code");
                if (result_code.ToUpper() == "SUCCESS")
                {
                    //直到这里，才能认为交易真正成功了，可以进行数据库操作

                    var order=await _orderRepository.UpdatePaymentStatusAsync(attach, cancelToken);
                    var shopUserList = await _userRepository.GetByShopIdAsync(order.ShopId.ToString(), cancelToken);
                    foreach (var item in shopUserList)
                    {
                        SignalR("2", order.ShopId.ToString(), item.Id.ToString(), order);
                    }
                    string content = "Success";

                    return new HttpResponseMessage { Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "text/plain") };
                }
            }
            else
            {
                //错误的订单处理

            }


            return new HttpResponseMessage { Content = new StringContent("", Encoding.GetEncoding("UTF-8"), "text/plain") };
        }

        #endregion

        /// <summary>
        /// SignalR推送
        /// </summary>
        /// <param name="type"></param>
        /// <param name="companyID"></param>
        /// <param name="employeeId"></param>
        /// <param name="order"></param>
        private void SignalR(string type, string companyID, string employeeId, Order order)
        {
            var conid = SignalRServerHub.ConnectionIds.FirstOrDefault(a => a.Key == type + "@" + companyID + "@" + employeeId).Value;
            if (!string.IsNullOrEmpty(conid))
            {
                SignalRServerHub.ClientList.Client(conid).getorderinfo(new 
                {
                    Id = order.Id.ToString(),
                    CreateOperationTime = order.CreateOperationTime.Value.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
                    order.ShopCommodityCount
                });
            }
        }
    }

}
