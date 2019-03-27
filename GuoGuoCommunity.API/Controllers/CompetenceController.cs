using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
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
        private readonly ITestRepository _testRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IUserRepository _userRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 权限
        /// </summary>
        /// <param name="testRepository"></param>
        /// <param name="menuRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="roleMenuRepository"></param>
        /// <param name="userRepository"></param>
        public CompetenceController(
            ITestRepository testRepository,
            IMenuRepository menuRepository,
            IRoleRepository roleRepository,
            IRoleMenuRepository roleMenuRepository,
            IUserRepository userRepository)
        {
            _testRepository = testRepository;
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _userRepository = userRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// pc端登陆
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/Login")]
        public async Task<ApiResult<LoginOutput>> Login([FromBody] LoginInput input, CancellationToken cancelToken)
        {
            try
            {
                var user = await _userRepository.GetAsync(
                    new UserDto
                    {
                        Name = input.Name,
                        Password = input.Pwd
                    });
                //产生 Token
                var token = _tokenManager.Create(user);
                //存入数据库

                await _userRepository.UpdateTokenAsync(
                    new UserDto
                    {
                        Id = user.Id.ToString(),
                        RefreshToken = token.Refresh_token
                    });

                if (user.Name == "admin")
                {
                    return new ApiResult<LoginOutput>(APIResultCode.Success, new LoginOutput
                    {
                        Roles = new string[1] { "authorityMax" },
                        Name = user.Name,
                        avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                        token = token.Access_token,
                        City = user.City,
                        Region = user.Region,
                        State = user.State
                        //refresh_token=token.refresh_token
                    });
                }

                var role_Menus = await _roleMenuRepository.GetByRoleIdAsync(user.RoleId, cancelToken);
                List<string> list = new List<string>();
                if (role_Menus != null)
                {
                    foreach (var item in role_Menus)
                    {
                        var menu = await _menuRepository.GetByIdAsync(item.MenuId, cancelToken);
                        list.Add(menu.Key);
                    }

                }
                return new ApiResult<LoginOutput>(APIResultCode.Success,
                    new LoginOutput
                    {
                        Roles = list.ToArray(),
                        Name = user.Name,
                        avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                        token = token.Access_token,
                        City = user.City,
                        Region = user.Region,
                        State = user.State
                        // refresh_token = token.refresh_token
                    });
            }
            catch (Exception e)
            {
                return new ApiResult<LoginOutput>(APIResultCode.Success_NoB, new LoginOutput { }, e.Message);
            }

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
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    City = user.City,
                    Region = user.Region,
                    State = user.State
                });
            }

            var role_Menus = await _roleMenuRepository.GetByRoleIdAsync(user.RoleId, cancelToken);
            List<string> list = new List<string>();
            if (role_Menus != null)
            {
                foreach (var item in role_Menus)
                {
                    var menu = await _menuRepository.GetByIdAsync(item.MenuId, cancelToken);
                    list.Add(menu.Key);
                }

            }
            return new ApiResult<LoginTokenOutput>(APIResultCode.Success,
                new LoginTokenOutput
                {
                    Roles = list.ToArray(),
                    Name = user.Name,
                    avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                    City = user.City,
                    Region = user.Region,
                    State = user.State
                });
        }

        #region 账号管理

        /// <summary>
        /// 添加街道办账号
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/addStreetOfficeUser")]
        public async Task<ApiResult<AddStreetOfficeUserOutput>> AddStreetOfficeUser([FromBody]AddStreetOfficeUserInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Unknown, new AddStreetOfficeUserOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Unknown, new AddStreetOfficeUserOutput { }, APIResultMessage.TokenError);
                }
                var entity = await _userRepository.AddStreetOfficeAsync(new UserDto
                {
                    Name = input.Name,
                    PhoneNumber = input.PhoneNumber,
                    //Password = input.Password,
                    State = input.State,
                    City = input.City,
                    Region = input.Region,
                    StreetOfficeId = input.StreetOfficeId,
                    DepartmentValue = Department.JieDaoBan.Value,
                    RoleId = input.RoleId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
                return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Success, new AddStreetOfficeUserOutput { Id = entity.Id.ToString() }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Success_NoB, new AddStreetOfficeUserOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询街道办列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/GetAllStreetOfficeUser")]
        public async Task<ApiResult<GetAllStreetOfficeUserOutput>> GetAllStreetOfficeUser([FromUri]GetAllStreetOfficeUserInput input, CancellationToken cancelToken)
        {
            try
            {
                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var data = await _userRepository.GetAllStreetOfficeAsync(new UserDto
                {
                    Name = input?.Name,
                    State = input.State,
                    City = input.City,
                    Region = input.Region,
                    StreetOfficeId = input.StreetOfficeId
                }, cancelToken);

                return new ApiResult<GetAllStreetOfficeUserOutput>(APIResultCode.Success, new GetAllStreetOfficeUserOutput
                {
                    List = data.Select(x => new GetUserOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                        City = x.City,
                        CommunityId = x.CommunityId,
                        CommunityName = x.CommunityName,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        PhoneNumber = x.PhoneNumber,
                        Region = x.Region,
                        RoleId = x.RoleId,
                        RoleName = x.RoleName,
                        State = x.State,
                        StreetOfficeId = x.StreetOfficeId,
                        StreetOfficeName = x.StreetOfficeName,
                        Password = x.Password
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllStreetOfficeUserOutput>(APIResultCode.Success_NoB, new GetAllStreetOfficeUserOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 修改街道办账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/updateStreetOfficeUser")]
        public async Task<ApiResult> UpdateStreetOfficeUser([FromBody]UpdateStreetOfficeUserInput input, CancellationToken cancellationToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }

                await _userRepository.UpdateAsync(new UserDto
                {
                    Id = input.Id,
                    Name = input.Name,
                    Password = input.Password,
                    PhoneNumber = input.PhoneNumber,
                    RoleId = input.RoleId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                });

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }

        }

        /// <summary>
        /// 添加物业账号
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/addPropertyUser")]
        public async Task<ApiResult<AddPropertyUserOutput>> AddPropertyUser([FromBody]AddPropertyUserInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddPropertyUserOutput>(APIResultCode.Unknown, new AddPropertyUserOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddPropertyUserOutput>(APIResultCode.Unknown, new AddPropertyUserOutput { }, APIResultMessage.TokenError);
                }
                var entity = await _userRepository.AddPropertyAsync(new UserDto
                {
                    Name = input.Name,
                    PhoneNumber = input.PhoneNumber,
                    Password = input.Password,
                    State = input.State,
                    City = input.City,
                    Region = input.Region,
                    StreetOfficeId = input.StreetOfficeId,
                    SmallDistrictId = input.SmallDistrictId,
                    CommunityId = input.CommunityId,
                    DepartmentValue = Department.WuYe.Value,
                    RoleId = input.RoleId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success, new AddPropertyUserOutput { Id = entity.Id.ToString() }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询物业列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/GetAllPropertyUser")]
        public async Task<ApiResult<GetAllPropertyUserOutput>> GetAllPropertyUser([FromUri]GetAllPropertyUserInput input, CancellationToken cancelToken)
        {
            try
            {
                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var data = await _userRepository.GetAllPropertyAsync(new UserDto
                {
                    Name = input?.Name,
                    State = input.State,
                    City = input.City,
                    Region = input.Region,
                    StreetOfficeId = input.StreetOfficeId,
                    SmallDistrictId = input.SmallDistrictId,
                    CommunityId = input.CommunityId
                }, cancelToken);

                return new ApiResult<GetAllPropertyUserOutput>(APIResultCode.Success, new GetAllPropertyUserOutput
                {
                    List = data.Select(x => new GetUserOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                        City = x.City,
                        CommunityId = x.CommunityId,
                        CommunityName = x.CommunityName,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        PhoneNumber = x.PhoneNumber,
                        Region = x.Region,
                        RoleId = x.RoleId,
                        RoleName = x.RoleName,
                        State = x.State,
                        StreetOfficeId = x.StreetOfficeId,
                        StreetOfficeName = x.StreetOfficeName,
                        Password = x.Password
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllPropertyUserOutput>(APIResultCode.Success_NoB, new GetAllPropertyUserOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 修改物业账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/updatePropertyUser")]
        public async Task<ApiResult> UpdatePropertyUser([FromBody]UpdatePropertyUserInput input, CancellationToken cancellationToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }

                await _userRepository.UpdateAsync(new UserDto
                {
                    Id = input.Id,
                    Name = input.Name,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    Password = input.Password,
                    RoleId = input.RoleId,
                    PhoneNumber = input.PhoneNumber
                });

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }

        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("用户Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _userRepository.DeleteAsync(new UserDto
                {
                    Id = id,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        #endregion

        #region 菜单管理

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        [HttpPost]
        [Route("menu/add")]
        public async Task<ApiResult<AddMenuOutput>> AddMenu([FromBody]AddMenuInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.Key))
                {
                    throw new NotImplementedException("菜单值信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("菜单名称信息为空！");
                }

                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddMenuOutput>(APIResultCode.Unknown, new AddMenuOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddMenuOutput>(APIResultCode.Unknown, new AddMenuOutput { }, APIResultMessage.TokenError);
                }

                return new ApiResult<AddMenuOutput>(APIResultCode.Success, new AddMenuOutput
                {
                    Id = (await _menuRepository.AddAsync(new MenuDto
                    {
                        Key = input.Key,
                        Name = input.Name,
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken)).Id.ToString()
                }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<AddMenuOutput>(APIResultCode.Success_NoB, new AddMenuOutput { }, e.Message);
            }

        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("menu/getAll")]
        public async Task<ApiResult<List<GetAllMenuOutput>>> GetAllMenu(CancellationToken cancelToken)
        {
            var data = (await _menuRepository.GetAllAsync(cancelToken)).Select(
                x => new GetAllMenuOutput
                {
                    Id = x.Id.ToString(),
                    Key = x.Key,
                    Name = x.Name
                }).ToList();
            return new ApiResult<List<GetAllMenuOutput>>
            {
                code = APIResultCode.Success,
                data = data
            };
        }

        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("menu/delete")]
        public async Task<ApiResult> DeleteMenu([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("菜单Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _menuRepository.DeleteAsync(new MenuDto
                {
                    Id = id,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        [HttpPost]
        [Route("role/add")]
        public async Task<ApiResult<AddRoleOutput>> AddRole([FromBody]AddRoleInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("角色名称信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.DepartmentValue))
                {
                    throw new NotImplementedException("角色部门值信息为空！");
                }
                if (string.IsNullOrWhiteSpace(Department.GetAll().Where(x => x.Value == input.DepartmentValue).Select(x => x.Name).FirstOrDefault()))
                {
                    throw new NotImplementedException("角色部门信息不准确！");
                }

                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddRoleOutput>(APIResultCode.Unknown, new AddRoleOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddRoleOutput>(APIResultCode.Unknown, new AddRoleOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _roleRepository.AddAsync(new RoleDto
                {
                    Name = input.Name,
                    DepartmentValue = input.DepartmentValue,
                    DepartmentName = Department.GetAll().Where(x => x.Value == input.DepartmentValue).Select(x => x.Name).FirstOrDefault(),
                    Description = input.Description,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddRoleOutput>(APIResultCode.Success, new AddRoleOutput { Id = entity.Id.ToString() }, APIResultMessage.Success);

            }
            catch (Exception)
            {
                return new ApiResult<AddRoleOutput>(APIResultCode.Unknown, new AddRoleOutput { }, APIResultMessage.TokenError);
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAll")]
        public async Task<ApiResult<List<GetAllRoleOutput>>> GetAllRole([FromUri]GetAllRoleInput input, CancellationToken cancelToken)
        {
            var data = (await _roleRepository.GetAllAsync(new RoleDto { DepartmentValue = input?.DepartmentValue, Name = input?.Name }, cancelToken)).Select(
                 x => new GetAllRoleOutput
                 {
                     Id = x.Id.ToString(),
                     Name = x.Name,
                     DepartmentName = x.DepartmentName,
                     DepartmentValue = x.DepartmentValue,
                     Description = x.Description
                 }).ToList();
            return new ApiResult<List<GetAllRoleOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 获取街道办用角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAllForStreetOffice")]
        public async Task<ApiResult<List<GetAllRoleOutput>>> GetAllRoleForStreetOffice(CancellationToken cancelToken)
        {
            try
            {
                var data = (await _roleRepository.GetAllAsync(new RoleDto { DepartmentValue = Department.JieDaoBan.Value }, cancelToken)).Select(
                 x => new GetAllRoleOutput
                 {
                     Id = x.Id.ToString(),
                     Name = x.Name,
                     DepartmentName = x.DepartmentName,
                     DepartmentValue = x.DepartmentValue,
                     Description = x.Description
                 }).ToList();
                return new ApiResult<List<GetAllRoleOutput>>(APIResultCode.Success, data);
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetAllRoleOutput>>(APIResultCode.Success_NoB, new List<GetAllRoleOutput>(), e.Message);
            }

        }

        /// <summary>
        /// 获取物业用角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAllForProperty")]
        public async Task<ApiResult<List<GetAllRoleOutput>>> GetAllRoleForProperty(CancellationToken cancelToken)
        {
            try
            {
                var data = (await _roleRepository.GetAllAsync(new RoleDto { DepartmentValue = Department.WuYe.Value }, cancelToken)).Select(
                x => new GetAllRoleOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    DepartmentName = x.DepartmentName,
                    DepartmentValue = x.DepartmentValue,
                    Description = x.Description
                }).ToList();
                return new ApiResult<List<GetAllRoleOutput>>(APIResultCode.Success, data);
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetAllRoleOutput>>(APIResultCode.Success_NoB, new List<GetAllRoleOutput>(), e.Message);
            }

        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("role/delete")]
        public async Task<ApiResult> DeleteRole([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("菜单Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _roleRepository.DeleteAsync(new RoleDto
                {
                    Id = id,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        #endregion

        #region 权限管理

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("roleMenu/add")]
        public async Task<ApiResult<AddRoleMenuOutput>> AddRoleMenu([FromBody]AddRoleMenuInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.MenuId))
                {
                    throw new NotImplementedException("菜单Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.RoleId))
                {
                    throw new NotImplementedException("角色Id信息为空！");
                }

                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddRoleMenuOutput>(APIResultCode.Unknown, new AddRoleMenuOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddRoleMenuOutput>(APIResultCode.Unknown, new AddRoleMenuOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _roleMenuRepository.AddAsync(new RoleMenuDto
                {
                    MenuId = input.MenuId,
                    RoleId = input.RoleId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
                return new ApiResult<AddRoleMenuOutput>(APIResultCode.Success, new AddRoleMenuOutput { Id = entity.Id.ToString() }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<AddRoleMenuOutput>(APIResultCode.Success_NoB, new AddRoleMenuOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 删除权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("roleMenu/delete")]
        public async Task<ApiResult> DeleteRoleMenu([FromBody]DeleteRoleMenuInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }

                if (string.IsNullOrWhiteSpace(input.MenuId))
                {
                    throw new NotImplementedException("菜单Id信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.RoleId))
                {
                    throw new NotImplementedException("角色Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _roleMenuRepository.DeleteAsync(new RoleMenuDto
                {
                    RoleId = input.RoleId,
                    MenuId = input.MenuId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
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
            var data = await _roleMenuRepository.GetByRoleIdAsync(input.RoleId, cancelToken);
            List<GetRoleMenusOutput> list = new List<GetRoleMenusOutput>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var menu = await _menuRepository.GetByIdAsync(item.MenuId, cancelToken);
                    list.Add(new GetRoleMenusOutput
                    {

                        RoleId = item.RolesId,
                        MenuId = item.MenuId,
                        Key = menu.Key,
                        Name = menu.Name
                    });
                }
            }
            return new ApiResult<List<GetRoleMenusOutput>>(APIResultCode.Success, list);

        }

        #endregion
    }
}
