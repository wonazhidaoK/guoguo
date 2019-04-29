using GuoGuoCommunity.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;

namespace Logs
{
    /// <summary>
    /// 异常筛选器
    /// </summary>
    public class AbnormalFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new AppLog());
            var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
            trace.Error(actionExecutedContext.Request, "Controller : " + actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + actionExecutedContext.ActionContext.ActionDescriptor.ActionName, actionExecutedContext.Exception);
            var exceptionType = actionExecutedContext.Exception.GetType();
            if (exceptionType == typeof(ValidationException))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(actionExecutedContext.Exception.Message), ReasonPhrase = "ValidationException" };
                throw new HttpResponseException(resp);
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }
            else
            {
                var oResponse = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                oResponse.Content = new StringContent("方法不被支持");
                oResponse.ReasonPhrase = "This Func is Not Supported";
                actionExecutedContext.Response = oResponse;
               // throw new ApiResult(APIResultCode.Success_NoB, actionExecutedContext.Exception.Message);
               // throw new HttpActionExecutedContext() ;
            }
            //base.OnException(actionExecutedContext);
        }
    }
}