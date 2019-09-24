using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    ///  权限
    /// </summary>
    public class CompetenceController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStationLetterRepository _stationLetterRepository;
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 权限
        /// </summary>
        public CompetenceController(
            IMenuRepository menuRepository,
            IRoleRepository roleRepository,
            IRoleMenuRepository roleMenuRepository,
            IUserRepository userRepository,
            IStationLetterRepository stationLetterRepository,
            ISmallDistrictRepository smallDistrictRepository,
            ITokenRepository tokenRepository)
        {
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _userRepository = userRepository;
            _stationLetterRepository = stationLetterRepository;
            _smallDistrictRepository = smallDistrictRepository;
            _tokenRepository = tokenRepository;
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
            var user = await _userRepository.GetIncludeAsync(new UserDto
            {
                Name = input.Name,
                Password = input.Pwd
            });

            //产生 Token
            var token = _tokenRepository.Create(user);
            //存入数据库

            await _userRepository.UpdateTokenAsync(new UserDto
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
                    State = user.State,
                    CommunityId = user.CommunityId,
                    CommunityName = user.CommunityName,
                    SmallDistrictId = user.SmallDistrictId,
                    SmallDistrictName = user.SmallDistrictName,
                    StreetOfficeId = user.StreetOfficeId,
                    StreetOfficeName = user.StreetOfficeName,
                    DepartmentName = "authorityMax",
                    DepartmentValue = "authorityMax",
                    ShopId = user.ShopId.ToString(),
                    Id = user.Id.ToString()
                });
            }

            var role_Menus = await _roleMenuRepository.GetByRoleIdAsync(user.RoleId, cancelToken);
            var menuList = await _menuRepository.GetByIdsAsync(role_Menus.Select(x => x.MenuId).ToList(), cancelToken);
            List<string> list = new List<string>();
            if (role_Menus.Any())
            {
                foreach (var item in role_Menus)
                {
                    var menu = menuList.Where(x => x.Id.ToString() == item.MenuId).FirstOrDefault();
                    list.Add(menu?.Key);
                }
            }
            else
            {
                return new ApiResult<LoginOutput>(APIResultCode.Success_NoB, new LoginOutput { }, "登陆账户角色未分配菜单浏览权限!");
            }
            bool IsHaveUnRead = false;
            string PropertyCompanyId = "";
            if (user.DepartmentValue == Department.WuYe.Value)
            {
                var letterList = await _stationLetterRepository.GetListAsync(new StationLetterDto
                {
                    SmallDistrictArray = user.SmallDistrictId,
                    OperationUserId = user.Id.ToString(),
                    ReadStatus = "UnRead"
                }, cancelToken);
                IsHaveUnRead = letterList.Any();

                var smallDistrict = await _smallDistrictRepository.GetAsync(user.SmallDistrictId, cancelToken);
                if (smallDistrict != null)
                {
                    PropertyCompanyId = smallDistrict.PropertyCompanyId.HasValue ? smallDistrict.PropertyCompanyId.Value.ToString() : "";
                }
            }
            string printerName = "";
            string activitySign = "";
            if (user.DepartmentValue == Department.Shop.Value)
            {
                if (user.Shop.IsDeleted)
                {
                    return new ApiResult<LoginOutput>(APIResultCode.Success_NoB, new LoginOutput { }, "商铺为注销状态，不允许登陆!");
                }
                printerName = user.Shop.PrinterName;
                activitySign = user.Shop.ActivitySign;
            }

            return new ApiResult<LoginOutput>(APIResultCode.Success, new LoginOutput
            {
                Roles = list.ToArray(),
                Name = user.Name,
                avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                token = token.Access_token,
                City = user.City,
                Region = user.Region,
                State = user.State,
                CommunityId = user.CommunityId,
                CommunityName = user.CommunityName,
                SmallDistrictId = user.SmallDistrictId,
                SmallDistrictName = user.SmallDistrictName,
                StreetOfficeId = user.StreetOfficeId,
                StreetOfficeName = user.StreetOfficeName,
                IsHaveUnRead = IsHaveUnRead,
                DepartmentValue = user.DepartmentValue,
                DepartmentName = user.DepartmentValue,
                ShopId = user.ShopId.ToString(),
                PropertyCompanyId = PropertyCompanyId,
                Id = user.Id.ToString(),
                PrinterName = printerName,
                ActivitySign = activitySign
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
            if (Authorization == null)
            {
                return new ApiResult<LoginTokenOutput>(APIResultCode.Unknown, new LoginTokenOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
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
                    State = user.State,
                    CommunityId = user.CommunityId,
                    CommunityName = user.CommunityName,
                    SmallDistrictId = user.SmallDistrictId,
                    SmallDistrictName = user.SmallDistrictName,
                    StreetOfficeId = user.StreetOfficeId,
                    StreetOfficeName = user.StreetOfficeName,
                    DepartmentName = "authorityMax",
                    DepartmentValue = "authorityMax",
                    ShopId = user.ShopId.ToString(),
                    Id = user.Id.ToString()
                });
            }

            var role_Menus = await _roleMenuRepository.GetByRoleIdAsync(user.RoleId, cancelToken);
            List<string> list = new List<string>();
            var menuList = await _menuRepository.GetByIdsAsync(role_Menus.Select(x => x.MenuId).ToList(), cancelToken);
            if (role_Menus != null)
            {
                foreach (var item in role_Menus)
                {
                    var menu = menuList.Where(x => x.Id.ToString() == item.MenuId).FirstOrDefault();
                    list.Add(menu.Key);
                }
            }

            bool IsHaveUnRead = false;
            string PropertyCompanyId = "";
            if (user.DepartmentValue == Department.WuYe.Value)
            {
                var letterList = await _stationLetterRepository.GetListAsync(new StationLetterDto
                {
                    SmallDistrictArray = user.SmallDistrictId,
                    OperationUserId = user.Id.ToString(),
                    ReadStatus = "UnRead"
                }, cancelToken);
                IsHaveUnRead = letterList.Any();
                var smallDistrict = await _smallDistrictRepository.GetAsync(user.SmallDistrictId, cancelToken);
                if (smallDistrict != null)
                {
                    PropertyCompanyId = smallDistrict.PropertyCompanyId.HasValue ? smallDistrict.PropertyCompanyId.Value.ToString() : "";
                }
            }
            string printerName = "";
            string activitySign = "";
            if (user.DepartmentValue == Department.Shop.Value)
            {
                if (user.Shop.IsDeleted)
                {
                    return new ApiResult<LoginTokenOutput>(APIResultCode.Success_NoB, new LoginTokenOutput { }, "商铺为注销状态，不允许登陆!");
                }
                var users = await _userRepository.GetIncludeAsync(new UserDto
                {
                    Name = user.PhoneNumber,
                    Password = user.Password
                });
                printerName = users.Shop.PrinterName;
                activitySign = users.Shop.ActivitySign;
            }
            return new ApiResult<LoginTokenOutput>(APIResultCode.Success, new LoginTokenOutput
            {
                Roles = list.ToArray(),
                Name = user.Name,
                avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                City = user.City,
                Region = user.Region,
                State = user.State,
                CommunityId = user.CommunityId,
                CommunityName = user.CommunityName,
                SmallDistrictId = user.SmallDistrictId,
                SmallDistrictName = user.SmallDistrictName,
                StreetOfficeId = user.StreetOfficeId,
                StreetOfficeName = user.StreetOfficeName,
                IsHaveUnRead = IsHaveUnRead,
                DepartmentValue = user.DepartmentValue,
                DepartmentName = user.DepartmentValue,
                ShopId = user.ShopId.ToString(),
                PropertyCompanyId = PropertyCompanyId,
                Id = user.Id.ToString(),
                PrinterName = printerName,
                ActivitySign = activitySign
            });
        }

        #region 账号管理

        /// <summary>
        /// 获取账号详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/Get")]
        public async Task<ApiResult<GetUserOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetUserOutput>(APIResultCode.Unknown, new GetUserOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                throw new NotImplementedException("token无效！");
            }

            var data = await _userRepository.GetIncludeAsync(id, cancelToken);



            return new ApiResult<GetUserOutput>(APIResultCode.Success, new GetUserOutput
            {
                Id = data.Id.ToString(),
                Account = data.Account,
                City = data.City,
                CommunityId = data.CommunityId,
                CommunityName = data.CommunityName,
                DepartmentName = data.DepartmentName,
                DepartmentValue = data.DepartmentValue,
                Name = data.Name,
                Password = data.Password,
                PhoneNumber = data.PhoneNumber,
                Region = data.Region,
                RoleId = data.RoleId,
                RoleName = data.RoleName,
                SmallDistrictId = data.SmallDistrictId,
                SmallDistrictName = data.SmallDistrictName,
                State = data.State,
                StreetOfficeId = data.StreetOfficeId,
                StreetOfficeName = data.StreetOfficeName
            });
        }

        #region 街道办账号管理

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

            if (Authorization == null)
            {
                return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Unknown, new AddStreetOfficeUserOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Account))
            {
                return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Success_NoB, new AddStreetOfficeUserOutput { }, "账户名称为空！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Unknown, new AddStreetOfficeUserOutput { }, APIResultMessage.TokenError);
            }
            if (!re.IsMatch(input.Account))
            {
                return new ApiResult<AddStreetOfficeUserOutput>(APIResultCode.Success_NoB, new AddStreetOfficeUserOutput { }, "账户名称输入格式不正确");
            }
            var entity = await _userRepository.AddStreetOfficeAsync(new UserDto
            {
                Account = input.Account,
                Name = input.Name,
                PhoneNumber = input.PhoneNumber,
                Password = input.Password,
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllStreetOfficeUserOutput>(APIResultCode.Unknown, new GetAllStreetOfficeUserOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                throw new NotImplementedException("token无效！");
            }
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

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllStreetOfficeUserOutput>(APIResultCode.Success, new GetAllStreetOfficeUserOutput
            {
                List = list.Select(x => new GetUserOutput
                {
                    Account = x.Account,
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
                }).ToList(),
                TotalCount = listCount
            });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            }, cancellationToken);

            return new ApiResult();
        }

        #endregion

        #region 物业账号管理

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
            if (Authorization == null)
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Unknown, new AddPropertyUserOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Account))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "账户名称为空！");
            }
            if (!re.IsMatch(input.Account))//验证数据是否匹配
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "账户名称输入格式不正确");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Unknown, new AddPropertyUserOutput { }, APIResultMessage.TokenError);
            }
            var entity = await _userRepository.AddPropertyAsync(new UserDto
            {
                Account = input.Account,
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllPropertyUserOutput>(APIResultCode.Unknown, new GetAllPropertyUserOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllPropertyUserOutput>(APIResultCode.Unknown, new GetAllPropertyUserOutput { }, APIResultMessage.TokenError);
            }
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

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllPropertyUserOutput>(APIResultCode.Success, new GetAllPropertyUserOutput
            {
                List = list.Select(x => new GetUserOutput
                {
                    Account = x.Account,
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
                }).ToList(),
                TotalCount = listCount
            });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            }, cancellationToken);

            return new ApiResult();
        }

        #endregion

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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("用户Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
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

        #region 商户账号管理

        /// <summary>
        /// 添加商户帐号
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/addShopUser")]
        public async Task<ApiResult<AddPropertyUserOutput>> AddShopUser([FromBody]AddShopUserInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Unknown, new AddPropertyUserOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Account))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "账户名称为空！");
            }
            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "商户Id信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "名称为空！");
            }
            if (string.IsNullOrWhiteSpace(input.PhoneNumber))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "手机号为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Password))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "密码为空！");
            }
            if (!re.IsMatch(input.Account))
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success_NoB, new AddPropertyUserOutput { }, "账户名称输入格式不正确");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddPropertyUserOutput>(APIResultCode.Unknown, new AddPropertyUserOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _userRepository.AddShopAsync(new UserDto
            {
                Name = input.Name,
                PhoneNumber = input.PhoneNumber,
                Password = input.Password,
                ShopId = input.ShopId,
                RoleId = input.RoleId,
                Account = input.Account,
                DepartmentValue = Department.Shop.Value,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddPropertyUserOutput>(APIResultCode.Success, new AddPropertyUserOutput { Id = entity.Id.ToString() }, APIResultMessage.Success);
        }

        /// <summary>
        /// 修改商户账号
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/updateShopUser")]
        public async Task<ApiResult> UpdateShopUser([FromBody]UpdateShopUserInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _userRepository.UpdateAsync(new UserDto
            {
                Id = input.Id,
                Name = input.Name,
                PhoneNumber = input.PhoneNumber,
                Password = input.Password,
                RoleId = input.RoleId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 查询商家账户列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/getAllShopUser")]
        public async Task<ApiResult<GetAllShopUserOutput>> GetAllShopUser([FromUri]GetAllShopUserInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllShopUserOutput>(APIResultCode.Unknown, new GetAllShopUserOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllShopUserOutput>(APIResultCode.Unknown, new GetAllShopUserOutput { }, APIResultMessage.TokenError);
            }
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }

            var data = await _userRepository.GetAllShopAsync(new UserDto
            {
                Name = input?.Name,
                PhoneNumber = input?.PhoneNumber,
                ShopId = input?.ShopId,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            }, cancelToken);

            List<GetAllShopUserOutputModel> list = data.List.Select(x => new GetAllShopUserOutputModel
            {
                Account = x.Account,
                Id = x.Id.ToString(),
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                RoleId = x.RoleId,
                RoleName = x.RoleName,
                Password = x.Password,
                ShopId = x.ShopId.ToString(),
                ShopName = x.Shop.Name,
                CreateTime = x.CreateOperationTime.Value
            }).ToList();

            return new ApiResult<GetAllShopUserOutput>(APIResultCode.Success, new GetAllShopUserOutput
            {
                List = list,
                TotalCount = data.Count
            });
        }

        ///// <summary>
        ///// 删除商户用户信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="cancelToken"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("user/deleteShopUser")]
        //[Obsolete]
        //public async Task<ApiResult> DeleteShopUser([FromUri]string id, CancellationToken cancelToken)
        //{
        //    if (Authorization == null)
        //    {
        //        return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
        //    }
        //    if (string.IsNullOrWhiteSpace(id))
        //    {
        //        throw new NotImplementedException("用户Id信息为空！");
        //    }

        //    var user = _tokenRepository.GetUser(Authorization);
        //    if (user == null)
        //    {
        //        return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
        //    }

        //    await _userRepository.DeleteAsync(new UserDto
        //    {
        //        Id = id,
        //        OperationTime = DateTimeOffset.Now,
        //        OperationUserId = user.Id.ToString()
        //    }, cancelToken);
        //    //await _shopRepository.DeleteAsync(new ShopDto
        //    //{
        //    //    UserId = id,
        //    //    OperationTime = DateTimeOffset.Now,
        //    //    OperationUserId = user.Id.ToString()
        //    //}, cancelToken);
        //    return new ApiResult();
        //}

        #endregion

        #endregion

        #region 菜单管理

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        [Obsolete]
        [HttpPost]
        [Route("menu/add")]
        public async Task<ApiResult<AddMenuOutput>> AddMenu([FromBody]AddMenuInput input, CancellationToken cancelToken)
        {
            if (string.IsNullOrWhiteSpace(input.Key))
            {
                throw new NotImplementedException("菜单值信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("菜单名称信息为空！");
            }
            if (Authorization == null)
            {
                return new ApiResult<AddMenuOutput>(APIResultCode.Unknown, new AddMenuOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
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

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("menu/getAll")]
        public async Task<ApiResult<List<GetAllMenuOutput>>> GetAllMenu([FromUri]GetAllMenuInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetAllMenuOutput>>(APIResultCode.Unknown, new List<GetAllMenuOutput> { }, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetAllMenuOutput>>(APIResultCode.Unknown, new List<GetAllMenuOutput> { }, APIResultMessage.TokenError);
            }
            var data = (await _menuRepository.GetAllAsync(new MenuDto
            {
                DepartmentValue = input.DepartmentValue
            }, cancelToken)).Select(x => new GetAllMenuOutput
            {
                Id = x.Id.ToString(),
                Key = x.Key,
                Name = x.Name
            }).ToList();
            return new ApiResult<List<GetAllMenuOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("menu/delete")]
        public async Task<ApiResult> DeleteMenu([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("菜单Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            if (Authorization == null)
            {
                return new ApiResult<AddRoleOutput>(APIResultCode.Unknown, new AddRoleOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddRoleOutput>(APIResultCode.Unknown, new AddRoleOutput { }, APIResultMessage.TokenError);
            }
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

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAll")]
        public async Task<ApiResult<GetAllRoleOutput>> GetAllRole([FromUri]GetAllRoleInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllRoleOutput>(APIResultCode.Unknown, new GetAllRoleOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllRoleOutput>(APIResultCode.Unknown, new GetAllRoleOutput { }, APIResultMessage.TokenError);
            }
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }
            int startRow = (input.PageIndex - 1) * input.PageSize;

            var data = (await _roleRepository.GetAllAsync(new RoleDto
            {
                DepartmentValue = input?.DepartmentValue,
                Name = input?.Name
            }, cancelToken));
            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);
            return new ApiResult<GetAllRoleOutput>(APIResultCode.Success, new GetAllRoleOutput
            {
                List = list.Select(x => new GetRoleOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    DepartmentName = x.DepartmentName,
                    DepartmentValue = x.DepartmentValue,
                    Description = x.Description
                }).ToList(),
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 获取街道办用角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAllForStreetOffice")]
        public async Task<ApiResult<List<GetRoleOutput>>> GetAllRoleForStreetOffice(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetRoleOutput>>(APIResultCode.Unknown, new List<GetRoleOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetRoleOutput>>(APIResultCode.Unknown, new List<GetRoleOutput> { }, APIResultMessage.TokenError);
            }
            var data = (await _roleRepository.GetAllAsync(new RoleDto { DepartmentValue = Department.JieDaoBan.Value }, cancelToken)).Select(x =>
            new GetRoleOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                DepartmentName = x.DepartmentName,
                DepartmentValue = x.DepartmentValue,
                Description = x.Description
            }).ToList();
            return new ApiResult<List<GetRoleOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 获取物业用角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAllForProperty")]
        public async Task<ApiResult<List<GetRoleOutput>>> GetAllRoleForProperty(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetRoleOutput>>(APIResultCode.Unknown, new List<GetRoleOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetRoleOutput>>(APIResultCode.Unknown, new List<GetRoleOutput> { }, APIResultMessage.TokenError);
            }
            var data = (await _roleRepository.GetAllAsync(new RoleDto { DepartmentValue = Department.WuYe.Value }, cancelToken)).Select(x =>
            new GetRoleOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                DepartmentName = x.DepartmentName,
                DepartmentValue = x.DepartmentValue,
                Description = x.Description
            }).ToList();
            return new ApiResult<List<GetRoleOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 获取商户用角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("role/getAllForShop")]
        public async Task<ApiResult<List<GetRoleOutput>>> GetAllRoleForShop(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetRoleOutput>>(APIResultCode.Unknown, new List<GetRoleOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetRoleOutput>>(APIResultCode.Unknown, new List<GetRoleOutput> { }, APIResultMessage.TokenError);
            }
            var data = (await _roleRepository.GetAllAsync(new RoleDto { DepartmentValue = Department.Shop.Value }, cancelToken)).Select(x =>
            new GetRoleOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                DepartmentName = x.DepartmentName,
                DepartmentValue = x.DepartmentValue,
                Description = x.Description
            }).ToList();
            return new ApiResult<List<GetRoleOutput>>(APIResultCode.Success, data);
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("菜单Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            if (string.IsNullOrWhiteSpace(input.MenuId))
            {
                throw new NotImplementedException("菜单Id信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.RoleId))
            {
                throw new NotImplementedException("角色Id信息为空！");
            }
            if (Authorization == null)
            {
                return new ApiResult<AddRoleMenuOutput>(APIResultCode.Unknown, new AddRoleMenuOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            if (Authorization == null)
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

            var user = _tokenRepository.GetUser(Authorization);
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
            if (Authorization == null)
            {
                return new ApiResult<List<GetRoleMenusOutput>>(APIResultCode.Unknown, new List<GetRoleMenusOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetRoleMenusOutput>>(APIResultCode.Unknown, new List<GetRoleMenusOutput> { }, APIResultMessage.TokenError);
            }
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
