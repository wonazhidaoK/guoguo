using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Service;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

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

            var container = new UnityContainer();
            config.EnableCors();
            container.RegisterType<ITestService, TestService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMenuService, MenuService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleService, RoleService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleMenuService, RoleMenuService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICityService, CityService>(new HierarchicalLifetimeManager());
            container.RegisterType<IStreetOfficeService, StreetOfficeService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISmallDistrictService, SmallDistrictService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommunityService, CommunityService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBuildingService, BuildingService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBuildingUnitService, BuildingUnitService>(new HierarchicalLifetimeManager());
            container.RegisterType<IComplaintTypeService, ComplaintTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVipOwnerService, VipOwnerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVipOwnerStructureService, VipOwnerStructureService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIndustryService, IndustryService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOwnerService, OwnerService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            config.Filters.Add(new Logs.LogFilterAttribute());
            config.Filters.Add(new Logs.AbnormalFilterAttribute());
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
