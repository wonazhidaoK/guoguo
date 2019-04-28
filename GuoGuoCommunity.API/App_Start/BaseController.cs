using Senparc.Weixin;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Http;

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

        ///// <summary>
        ///// 
        ///// </summary>
        //public int loginid { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string loginname { get; set; }

        /////// <summary>
        /////// 
        /////// </summary>
        ////public BaseBll baseBll { get; set; }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="controllerContext"></param>
        //protected override void Initialize(HttpControllerContext controllerContext)
        //{
        //    //初始化请求上下文
        //    base.Initialize(controllerContext);
        //    try
        //    {
        //        new SortedDictionary<string, string>();
        //        string username = string.Empty;
        //        HttpRequestHeaders headers = controllerContext.Request.Headers;
        //        if (headers.Contains("e"))
        //        {
        //            //text = (headers.GetValues("e").FirstOrDefault<string>().ToString() ?? string.Empty);
        //            //text = System.Web.HttpUtility.UrlDecode(username);
        //        }
        //    //    UserInfoEntity userInfo = new LoginBll().GetUserInfo(username);
        //    //    this.loginid = userInfo.LoginID;
        //    //    this.loginname = userInfo.LoginName;
        //    //    List<UserAuthorityEntity> tempList = userInfo.UserRole.UserAuthority;
        //    //    //不存在安全问题 后续文章有权限验证
        //    //    if (tempList.Where(c => c.AuthorityName == "权限名称").ToList().Count > 0)
        //    //    {
        //    //        //调用一个有权限的bll层
        //    //        this.baseBll = new SeniorBll();
        //    //    }
        //    //    else
        //    //    {
        //    //        //调用一个没有权限的bll层
        //    //        this.baseBll = new OrdinaryBll();
        //    //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogHelper.WriteErrorLog("Initialize", ex);
        //    }
        //}
        ///// <summary>
        ///// 设置action返回信息
        ///// </summary>
        ///// <param name="result">返回实体</param>
        ///// <returns></returns>
        //protected HttpResponseMessage GetHttpResponseMessage(object result)
        //{
        //    //BaseResponseEntity<object> responseBaseEntity = new BaseResponseEntity<object>(0, result, string.Empty);
        //    return new HttpResponseMessage();
        //    //{
        //    //    Content =
        //    //       new StringContent(JsonConvert.SerializeObject(responseBaseEntity, dtConverter), System.Text.Encoding.UTF8,
        //    //           "application/json")
        //    //};
        //}
        ///// <summary>
        ///// 设置action返回信息
        ///// </summary>
        ///// <param name="result">返回实体</param>
        ///// <param name="msg">返回的信息参数</param>
        ///// <returns></returns>
        //protected HttpResponseMessage GetHttpResponseMessage(object result, ref string msg)
        //{
        //    //BaseResponseEntity<object> responseBaseEntity = new BaseResponseEntity<object>(0, result, msg ?? string.Empty);
        //    return new HttpResponseMessage();
        //    //{
        //    //    Content =
        //    //       new StringContent(JsonConvert.SerializeObject(responseBaseEntity, dtConverter), System.Text.Encoding.UTF8,
        //    //           "application/json")
        //    //};
        //}
    }
}