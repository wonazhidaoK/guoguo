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
            container.RegisterType<ITestRepository, TestRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMenuRepository, MenuRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleRepository, RoleRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleMenuRepository, RoleMenuRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICityRepository, CityRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IStreetOfficeRepository, StreetOfficeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISmallDistrictRepository, SmallDistrictRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommunityRepository, CommunityRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBuildingRepository, BuildingRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBuildingUnitRepository, BuildingUnitRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IComplaintTypeRepository, ComplaintTypeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVipOwnerRepository, VipOwnerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVipOwnerStructureRepository, VipOwnerStructureRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IIndustryRepository, IndustryRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOwnerRepository, OwnerRepository>(new HierarchicalLifetimeManager());
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

           // AutoFacBootStrapper.CoreAutoFacInit();
        }
    }
}
