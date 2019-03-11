using GuoGuoCommunity.API;
using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;

namespace Logs
{
    /// <summary>
    /// 日志筛选器
    /// </summary>
    public class LogFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new AppLog());
            var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
            trace.Info(actionContext.Request, "Controller : " + actionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + actionContext.ActionDescriptor.ActionName, "JSON", actionContext.ActionArguments);
            //base.OnActionExecuting(actionContext);
        }
    }
}