using Senparc.Weixin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GuoGuoCommunity.API
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// 小程序AppID
        /// </summary>
        public static readonly string GuoGuoCommunity_WxOpenAppId = ConfigurationManager.AppSettings["GuoGuoCommunity_WxOpenAppId"];

        /// <summary>
        /// 微信AppID
        /// </summary>
        public static readonly string AppId = ConfigurationManager.AppSettings["GuoGuoCommunity_AppId"];

        /// <summary>
        /// 微信推送公告模板Id
        /// </summary>
        public static readonly string AnnouncementTemplateId = ConfigurationManager.AppSettings["AnnouncementTemplateId"];

        /// <summary>
        /// 微信推送下单成功模板Id
        /// </summary>
        public static readonly string OrderTemplateId = ConfigurationManager.AppSettings["OrderTemplateId"];

        /// <summary>
        /// 创建账户正则
        /// </summary>
        public readonly Regex re = new Regex(@"^[a-zA-Z0-9_]{1,}$");

        /// <summary>
        /// 阿里云接口地址
        /// </summary>
        public static readonly string ALiYunApiUrl = ConfigurationManager.AppSettings["ALiYunApiUrl"];

        /// <summary>
        /// 阿里云AppCode
        /// </summary>
        public static readonly string ALiYunApiAppCode = ConfigurationManager.AppSettings["ALiYunApiAppCode"];

        /// <summary>
        /// 微信推送认证结果模板Id
        /// </summary>
        public static readonly string OwnerCertificationRecordTemplateId = ConfigurationManager.AppSettings["OwnerCertificationRecordTemplateId"];

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Host = ConfigurationManager.AppSettings["Host"];

        /// <summary>
        /// 
        /// </summary>
        public static readonly string Agreement = ConfigurationManager.AppSettings["Agreement"];

        /// <summary>
        /// 微信推送创建投票模板Id
        /// </summary>
        public static readonly string VoteCreateTemplateId = ConfigurationManager.AppSettings["VoteCreateTemplateId"];

        /// <summary>
        /// 微信推送投票结果模板Id
        /// </summary>
        public static readonly string VoteResultTemplateId = ConfigurationManager.AppSettings["VoteResultTemplateId"];

        /// <summary>
        /// 微信推送高级认证通过模板Id
        /// </summary>
        public static readonly string VipOwnerCertificationRecordTemplateId = ConfigurationManager.AppSettings["VipOwnerCertificationRecordTemplateId"];

        /// <summary>
        /// 令牌
        /// </summary>
        public static readonly string Token = ConfigurationManager.AppSettings["GuoGuoCommunity_Token"];//与微信公众账号后台的Token设置保持一致，区分大小写。

        /// <summary>
        /// AESKey
        /// </summary>
        public static readonly string EncodingAESKey = ConfigurationManager.AppSettings["GuoGuoCommunity_EncodingAESKey"];//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。

        /// <summary>
        /// 微信Secret
        /// </summary>
        public static readonly string Secret = Config.SenparcWeixinSetting.WeixinAppSecret;

        /// <summary>
        /// 小程序Secret
        /// </summary>
        public static readonly string GuoGuoCommunity_WxOpenAppSecret = ConfigurationManager.AppSettings["GuoGuoCommunity_WxOpenAppSecret"];

        /// <summary>
        /// 支付Id
        /// </summary>
        public string PayV3_MchId = ConfigurationManager.AppSettings["PayV3_MchId"];

        /// <summary>
        /// 支付key
        /// </summary>
        public string PayV3_Key = ConfigurationManager.AppSettings["PayV3_Key"];

        /// <summary>
        /// 
        /// </summary>
        public static string Authorization = "";

        private static readonly object Locker = new object();
        private static short _sn = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            //初始化请求上下文
            base.Initialize(controllerContext);
            try
            {
                new SortedDictionary<string, string>();
                string username = string.Empty;
                HttpRequestHeaders headers = controllerContext.Request.Headers;
                Authorization = HttpContext.Current.Request.Headers["Authorization"];
                // token = HttpContext.Current.Request.Headers["Authorization"];
                //if (token == null)
                //{
                //    return new ApiResult(APIResultCode.Unknown, new AddVipOwnerAnnouncementOutput { }, APIResultMessage.TokenNull);
                //}
                if (headers.Contains("e"))
                {
                    //text = (headers.GetValues("e").FirstOrDefault<string>().ToString() ?? string.Empty);
                    //text = System.Web.HttpUtility.UrlDecode(username);
                }
                //    UserInfoEntity userInfo = new LoginBll().GetUserInfo(username);
                //    this.loginid = userInfo.LoginID;
                //    this.loginname = userInfo.LoginName;
                //    List<UserAuthorityEntity> tempList = userInfo.UserRole.UserAuthority;
                //    //不存在安全问题 后续文章有权限验证
                //    if (tempList.Where(c => c.AuthorityName == "权限名称").ToList().Count > 0)
                //    {
                //        //调用一个有权限的bll层
                //        this.baseBll = new SeniorBll();
                //    }
                //    else
                //    {
                //        //调用一个没有权限的bll层
                //        this.baseBll = new OrdinaryBll();
                //    }
            }
            catch (Exception)
            {
                //LogHelper.WriteErrorLog("Initialize", ex);
            }
        }

        /// <summary>
        /// 生成编号
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        protected string GenerateCode(string sign)
        {
            lock (Locker)  //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。 
            {
                if (_sn == short.MaxValue)
                {
                    _sn = 0;
                }
                else
                {
                    _sn++;
                }

                //Thread.Sleep(50);

                return sign + DateTime.Now.ToString("yyyyMMddHHmmss") + (_sn.ToString().PadLeft(5, '0'));
            }
        }
    }
}