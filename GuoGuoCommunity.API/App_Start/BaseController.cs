using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GuoGuoCommunity.API.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public int loginid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string loginname { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public BaseBll baseBll { get; set; }


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
            catch (Exception ex)
            {
                //LogHelper.WriteErrorLog("Initialize", ex);
            }
        }
        /// <summary>
        /// 设置action返回信息
        /// </summary>
        /// <param name="result">返回实体</param>
        /// <returns></returns>
        protected HttpResponseMessage GetHttpResponseMessage(object result)
        {
            //BaseResponseEntity<object> responseBaseEntity = new BaseResponseEntity<object>(0, result, string.Empty);
            return new HttpResponseMessage();
            //{
            //    Content =
            //       new StringContent(JsonConvert.SerializeObject(responseBaseEntity, dtConverter), System.Text.Encoding.UTF8,
            //           "application/json")
            //};
        }
        /// <summary>
        /// 设置action返回信息
        /// </summary>
        /// <param name="result">返回实体</param>
        /// <param name="msg">返回的信息参数</param>
        /// <returns></returns>
        protected HttpResponseMessage GetHttpResponseMessage(object result, ref string msg)
        {
            //BaseResponseEntity<object> responseBaseEntity = new BaseResponseEntity<object>(0, result, msg ?? string.Empty);
            return new HttpResponseMessage();
            //{
            //    Content =
            //       new StringContent(JsonConvert.SerializeObject(responseBaseEntity, dtConverter), System.Text.Encoding.UTF8,
            //           "application/json")
            //};
        }
    }
}