using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    ///  权限
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompetenceController : ApiController
    {
        private readonly ITestService _testService;
        private readonly IMenuService _menuService;
        private readonly IRoleService _roleService;
        private readonly IRoleMenuService _roleMenuService;
        private readonly IUserService _userService;
        private TokenManager _tokenManager;

        /// <summary>
        /// 权限
        /// </summary>
        /// <param name="testService"></param>
        /// <param name="menuService"></param>
        /// <param name="roleService"></param>
        /// <param name="roleMenuService"></param>
        /// <param name="userService"></param>
        public CompetenceController(
            ITestService testService,
            IMenuService menuService,
            IRoleService roleService,
            IRoleMenuService roleMenuService,
            IUserService userService)
        {
            _testService = testService;
            _menuService = menuService;
            _roleService = roleService;
            _roleMenuService = roleMenuService;
            _userService = userService;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        [HttpPost]
        [Route("menu/add")]
        public async Task<ApiResult> AddMenu([FromBody]AddMenuInput input, CancellationToken cancelToken)
        {
            await _menuService.AddAsync(new MenuDto
            {
                Kay = input.Kay,
                Name = input.Name
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("menu/getAll")]
        public async Task<ApiResult<List<GetAllMenuOutput>>> GetAllMenu(CancellationToken cancelToken)
        {
            var data = (await _menuService.GetAllAsync(cancelToken)).Select(
                x => new GetAllMenuOutput
                {
                    Id = x.Id.ToString(),
                    Kay = x.Kay,
                    Name = x.Name
                }).ToList();
            return new ApiResult<List<GetAllMenuOutput>>
            {
                code = APIResultCode.Success,
                data = data
            };
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        [HttpPost]
        [Route("role/add")]
        public async Task<ApiResult> AddRole([FromBody]AddRoleInput input, CancellationToken cancelToken)
        {
            await _roleService.AddAsync(new RoleDto
            {
                Name = input.Name
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAll")]
        public async Task<ApiResult<List<GetAllRoleOutput>>> GetAllRole(CancellationToken cancelToken)
        {
            var data = (await _roleService.GetAllAsync(cancelToken)).Select(
                 x => new GetAllRoleOutput
                 {
                     Id = x.Id.ToString(),
                     Name = x.Name
                 }).ToList();
            return new ApiResult<List<GetAllRoleOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("roleMenu/add")]
        public async Task<ApiResult> AddRoleMenu([FromBody]AddRoleMenuInput input, CancellationToken cancelToken)
        {
            await _roleMenuService.AddAsync(new RoleMenuDto
            {
                MenuId = input.MenuId,
                RolesId = input.RolesId
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 获取角色菜单权限
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("roleMenu/getRoleMenus")]
        public async Task<ApiResult<List<GetRoleMenusOutput>>> GetRoleMenus([FromUri]GetRoleMenusInput input, CancellationToken cancelToken)
        {
            var data = await _roleMenuService.GetByRoleIdAsync(input.RoleId, cancelToken);
            List<GetRoleMenusOutput> list = new List<GetRoleMenusOutput>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var menu = await _menuService.GetByIdAsync(item.MenuId, cancelToken);
                    list.Add(new GetRoleMenusOutput
                    {

                        RoleId = item.RolesId,
                        MenuId = item.MenuId,
                        IsDisplayed = item.IsDisplayed,
                        Key = menu.Kay,
                        Name = menu.Name
                    });
                }
            }
            return new ApiResult<List<GetRoleMenusOutput>>(APIResultCode.Success, list);

        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/addUser")]
        public async Task<ApiResult> AddUser([FromBody]AddUserInput input, CancellationToken cancelToken)
        {
            await _userService.AddAsync(new UserDto
            {
                Name = input.Name,
                RolesId = input.RolesId,
                PhoneNumber = input.PhoneNumber,
                RoleName = input.RoleName
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/Login")]
        public async Task<ApiResult<LoginOutput>> Login([FromBody]LoginInput input, CancellationToken cancelToken)
        {
            var user = await _userService.GetAsync(
               new UserDto
               {
                   Name = input.Name,
                   Password = input.Pwd
               });
            //產生 Token
            var token = _tokenManager.Create(user);
            //需存入資料庫

            await _userService.UpdateTokenAsync(
                new UserDto
                {
                    Id = user.Id.ToString(),
                    RefreshToken = token.refresh_token
                });

            if (user.Name == "admin")
            {
                return new ApiResult<LoginOutput>(APIResultCode.Success, new LoginOutput
                {
                    Roles = new string[1] { "authorityMax" },
                    Name = user.Name,
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    token = token.access_token,
                    //refresh_token=token.refresh_token
                });
            }

            var role_Menus = await _roleMenuService.GetByRoleIdAsync(user.RoleId, cancelToken);
            List<string> list = new List<string>();
            if (role_Menus != null)
            {
                foreach (var item in role_Menus)
                {
                    var menu = await _menuService.GetByIdAsync(item.MenuId, cancelToken);
                    list.Add(menu.Kay);
                }

            }
            return new ApiResult<LoginOutput>(APIResultCode.Success,
                new LoginOutput
                {
                    Roles = list.ToArray(),
                    Name = user.Name,
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    token = token.access_token,
                    // refresh_token = token.refresh_token
                });
        }

        /// <summary>
        /// 从Token获取用户信息
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/LoginToken")]
        public async Task<ApiResult<LoginTokenOutput>> LoginToken(CancellationToken cancelToken)
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];
            var user = _tokenManager.GetUser(token);
            if (user == null)
            {
                throw new NotImplementedException("token无效！");
            }
            if (user.Name == "admin")
            {
                return new ApiResult<LoginTokenOutput>(APIResultCode.Success, new LoginTokenOutput
                {
                    Roles = new string[1] { "authorityMax" },
                    Name = user.Name,
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif"
                });
            }

            var role_Menus = await _roleMenuService.GetByRoleIdAsync(user.RoleId, cancelToken);
            List<string> list = new List<string>();
            if (role_Menus != null)
            {
                foreach (var item in role_Menus)
                {
                    var menu = await _menuService.GetByIdAsync(item.MenuId, cancelToken);
                    list.Add(menu.Kay);
                }

            }
            return new ApiResult<LoginTokenOutput>(APIResultCode.Success,
                new LoginTokenOutput
                {
                    Roles = list.ToArray(),
                    Name = user.Name,
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif"
                });
        }
    }
}
