using Logs;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.Filters.Add(new LogFilterAttribute());
            config.Filters.Add(new AbnormalFilterAttribute());
            //config.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{action}/{content}",
                defaults: new { ApiController = "Message", Action = "Post", content = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
               name: "DefaultApi3",
               routeTemplate: "{controller}/{action}",
               defaults: new { ApiController = "WX" }
               );

            AutoFacBootStrapper.CoreAutoFacInit();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            });
        }

        class CancelledTaskBugWorkaroundMessageHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
                return response;
            }
        }
    }
}
