using Logs;
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

            //var container = new UnityContainer();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            //container.RegisterType<ITestRepository, TestRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IMenuRepository, MenuRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IRoleRepository, RoleRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IRoleMenuRepository, RoleMenuRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<ICityRepository, CityRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IStreetOfficeRepository, StreetOfficeRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<ISmallDistrictRepository, SmallDistrictRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<ICommunityRepository, CommunityRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IBuildingRepository, BuildingRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IBuildingUnitRepository, BuildingUnitRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IComplaintTypeRepository, ComplaintTypeRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IVipOwnerRepository, VipOwnerRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IVipOwnerStructureRepository, VipOwnerStructureRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IIndustryRepository, IndustryRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IOwnerRepository, OwnerRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IUploadRepository, UploadRepository>(new HierarchicalLifetimeManager());
            //config.DependencyResolver = new UnityResolver(container);




            config.Filters.Add(new LogFilterAttribute());
            config.Filters.Add(new AbnormalFilterAttribute());

            config.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());
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
        }

        class CancelledTaskBugWorkaroundMessageHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

                // Try to suppress response content when the cancellation token has fired; ASP.NET will log to the Application event log if there's content in this case.
                if (cancellationToken.IsCancellationRequested)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                return response;
            }
        }
    }
}
