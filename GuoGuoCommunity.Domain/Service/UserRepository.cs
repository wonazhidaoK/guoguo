using EntityFramework.Extensions;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> AddStreetOfficeAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.StreetOfficeId, out var streetOfficeId))
                {
                    throw new NotImplementedException("街道办Id信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == streetOfficeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("街道办信息不存在！");
                }

                if (!Guid.TryParse(dto.RoleId, out var roleId))
                {
                    throw new NotImplementedException("角色Id信息不正确！");
                }
                var role = await db.User_Roles.Where(x => x.Id == roleId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (role == null)
                {
                    throw new NotImplementedException("角色信息不存在！");
                }
                if (string.IsNullOrWhiteSpace(Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault()))
                {
                    throw new NotImplementedException("部门信息不存在！");
                }
                //var user = await db.Users.Where(x => (x.Name == dto.Name || x.PhoneNumber == dto.PhoneNumber) && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if ((await db.Users.Where(x => x.Account == dto.Account && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该账号已存在！");
                }
                if ((await db.Users.Where(x => x.PhoneNumber == dto.PhoneNumber && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该手机号已注册过平台内账户！");
                }
                var entity = db.Users.Add(new User
                {
                    Account = dto.Account,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Password = dto.Password,
                    RoleId = dto.RoleId,
                    RoleName = role.Name,
                    State = dto.State,
                    City = dto.City,
                    Region = dto.Region,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = streetOffice.Name,
                    DepartmentValue = dto.DepartmentValue,
                    DepartmentName = Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault(),
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task<User> AddPropertyAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.StreetOfficeId, out var streetOfficeId))
                {
                    throw new NotImplementedException("街道办Id信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == streetOfficeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("街道办信息不存在！");
                }

                if (!Guid.TryParse(dto.CommunityId, out var communityId))
                {
                    throw new NotImplementedException("社区Id信息不正确！");
                }
                var communitie = await db.Communities.Where(x => x.Id == communityId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (communitie == null)
                {
                    throw new NotImplementedException("社区信息不存在！");
                }

                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                if (!Guid.TryParse(dto.RoleId, out var roleId))
                {
                    throw new NotImplementedException("角色Id信息不正确！");
                }
                var role = await db.User_Roles.Where(x => x.Id == roleId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (role == null)
                {
                    throw new NotImplementedException("角色信息不存在！");
                }
                if (string.IsNullOrWhiteSpace(Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault()))
                {
                    throw new NotImplementedException("部门信息不存在！");
                }
                if ((await db.Users.Where(x => x.Account == dto.Account && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该账号已存在！");
                }
                if ((await db.Users.Where(x => x.PhoneNumber == dto.PhoneNumber && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该手机号已注册过平台内账户！");
                }
                var entity = db.Users.Add(new User
                {
                    Account = dto.Account,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Password = dto.Password,
                    RoleId = dto.RoleId,
                    RoleName = role.Name,
                    State = dto.State,
                    City = dto.City,
                    Region = dto.Region,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = streetOffice.Name,
                    CommunityId = dto.CommunityId,
                    CommunityName = communitie.Name,
                    SmallDistrictId = dto.SmallDistrictId,
                    SmallDistrictName = smallDistrict.Name,
                    DepartmentValue = dto.DepartmentValue,
                    DepartmentName = Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault(),
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("用户Id信息不正确！");
                }
                var users = await db.Users.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (users == null)
                {
                    throw new NotImplementedException("该用户不存在！");
                }

                users.LastOperationTime = dto.OperationTime;
                users.LastOperationUserId = dto.OperationUserId;
                users.DeletedTime = dto.OperationTime;
                users.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

       

        public async Task<List<User>> GetAllPropertyAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Users.Where(x => x.IsDeleted == false && x.DepartmentValue == Department.WuYe.Value).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.State))
                {
                    list = list.Where(x => x.State == dto.State).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.City))
                {
                    list = list.Where(x => x.City == dto.City).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Region))
                {
                    list = list.Where(x => x.Region == dto.Region).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.StreetOfficeId))
                {
                    list = list.Where(x => x.StreetOfficeId == dto.StreetOfficeId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.CommunityId))
                {
                    list = list.Where(x => x.CommunityId == dto.CommunityId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    list = list.Where(x => x.SmallDistrictId == dto.SmallDistrictId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }

                return list;
            }
        }

        public async Task<List<User>> GetAllStreetOfficeAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Users.Where(x => x.IsDeleted == false && x.DepartmentValue == Department.JieDaoBan.Value).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.State))
                {
                    list = list.Where(x => x.State == dto.State).ToList();
                }

                if (!string.IsNullOrWhiteSpace(dto.City))
                {
                    list = list.Where(x => x.City == dto.City).ToList();
                }

                if (!string.IsNullOrWhiteSpace(dto.Region))
                {
                    list = list.Where(x => x.Region == dto.Region).ToList();
                }

                if (!string.IsNullOrWhiteSpace(dto.StreetOfficeId))
                {
                    list = list.Where(x => x.StreetOfficeId == dto.StreetOfficeId).ToList();
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }

                return list;
            }
        }

        public async Task UpdateAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid uid))
                {
                    throw new NotImplementedException("用户Id信息不正确！");
                }
                var user = await db.Users.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("该用户不存在！");
                }
                if (!Guid.TryParse(dto.RoleId, out var roleId))
                {
                    throw new NotImplementedException("角色Id信息不正确！");
                }
                var role = await db.User_Roles.Where(x => x.Id == roleId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (role == null)
                {
                    throw new NotImplementedException("角色信息不存在！");
                }
                var entity = await db.Users.Where(x => x.PhoneNumber == dto.PhoneNumber && x.IsDeleted == false && x.Id != uid).FirstOrDefaultAsync(token);
                if (entity != null)
                {
                    throw new NotImplementedException("该手机号已存在！");
                }
                user.Name = dto.Name;
                user.Password = dto.Password;
                user.PhoneNumber = dto.PhoneNumber;
                user.RoleId = dto.RoleId;
                user.RoleName = role.Name;
                user.LastOperationTime = dto.OperationTime;
                user.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task UpdateTokenAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid uid))
                {
                    throw new NotImplementedException("用户Id信息不正确！");
                }
                var user = await db.Users.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("该用户不存在！");
                }
                user.RefreshToken = dto.RefreshToken;

                await db.SaveChangesAsync(token);
            }
        }

        public async Task<User> AddWeiXinAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var user = await db.Users.Where(x => x.OpenId == dto.OpenId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user != null)
                {
                    throw new NotImplementedException("该用户已存在！");
                }
                var entity = db.Users.Add(new User
                {
                    //Account = dto.Account,
                    OpenId = dto.OpenId,
                    UnionId = dto.UnionId,
                    DepartmentValue = Department.YeZhu.Value,
                    DepartmentName = Department.YeZhu.Name,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                    //Name = dto.Name,
                    //PhoneNumber = dto.PhoneNumber,
                    //RoleId = dto.RoleId,
                    //RoleName = dto.RoleName
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task<User> GetForOpenIdAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Users.Where(x => x.OpenId == dto.OpenId).FirstOrDefaultAsync(token);
            }
        }

        public async Task<User> GetForUnionIdAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Users.Where(x => x.UnionId == dto.UnionId).FirstOrDefaultAsync(token);
            }
        }

        public async Task<User> GetForIdAsync(string id, CancellationToken token = default)
        {
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    if (Guid.TryParse(id, out var uid))
                    {
                        return await db.Users.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                    }
                    throw new NotImplementedException("该用户Id信息不正确！");
                }
            }
            catch (Exception)
            {
                return new User();
            }
        }

        public async Task<List<User>> GetByIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Users.Where(x => ids.Contains(x.Id.ToString())).ToListAsync(token);
            }
        }

        #region 事件

        public void OnSubscribe(CommunityIncrementer incrementer)
        {
            incrementer.CommunityEvent += CommunityChanging;
        }

        public async void CommunityChanging(GuoGuoCommunityContext dbs, Community community, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Users.Where(x => x.CommunityId == community.Id.ToString()).UpdateAsync(x => new User { CommunityName = community.Name });
            }
        }

        public void OnSubscribe(StreetOfficeIncrementer incrementer)
        {
            incrementer.StreetOfficeEvent += StreetOfficeChanging;//在发布者私有委托里增加方法
        }

        public async void StreetOfficeChanging(GuoGuoCommunityContext dbs, StreetOffice streetOffice, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Users.Where(x => x.StreetOfficeId == streetOffice.Id.ToString()).UpdateAsync(x => new User { StreetOfficeName = streetOffice.Name });
            }
        }

        public void OnSubscribe(SmallDistrictIncrementer incrementer)
        {
            incrementer.SmallDistrictEvent += SmallDistrictChanging;//在发布者私有委托里增加方法
        }

        public async void SmallDistrictChanging(GuoGuoCommunityContext dbs, SmallDistrict smallDistrict, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Users.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new User { SmallDistrictName = smallDistrict.Name });
            }
        }

        #endregion


        public async Task<User> AddShopAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.RoleId, out var roleId))
                {
                    throw new NotImplementedException("角色Id信息不正确！");
                }
                var role = await db.User_Roles.Where(x => x.Id == roleId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (role == null)
                {
                    throw new NotImplementedException("角色信息不存在！");
                }
                if (string.IsNullOrWhiteSpace(Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault()))
                {
                    throw new NotImplementedException("部门信息不存在！");
                }

                if ((await db.Users.Where(x => x.Account == dto.Account && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该账号已存在！");
                }
                if ((await db.Users.Where(x => x.PhoneNumber == dto.PhoneNumber && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该手机号已注册过平台内账户！");
                }

                if (!Guid.TryParse(dto.ShopId, out var shopId))
                {
                    throw new NotImplementedException("角色Id信息不正确！");
                }
                var shop = await db.Shops.Where(x => x.Id == shopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shop == null)
                {
                    throw new NotImplementedException("商户信息不存在！");
                }
                var entity = db.Users.Add(new User
                {
                    Account = dto.Account,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Password = dto.Password,
                    RoleId = dto.RoleId,
                    RoleName = role.Name,
                    DepartmentValue = dto.DepartmentValue,
                    DepartmentName = Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault(),
                    ShopId = shopId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task<UserPageDto> GetAllShopAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.Users.Include(x => x.Shop).Where(x => x.IsDeleted == false && x.DepartmentValue == Department.Shop.Value);

                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                {
                    list = list.Where(x => x.PhoneNumber.Contains(dto.PhoneNumber));
                }
                if (!string.IsNullOrWhiteSpace(dto.ShopId))
                {
                    list = list.Where(x => x.ShopId.ToString() == dto.ShopId);
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name));
                }
                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<User> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                UserPageDto pagelist = new UserPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public async Task<User> GetIncludeAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var user = await db.Users.Include(x => x.Shop).Where(x => (x.Account == dto.Name || x.PhoneNumber == dto.Name) && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("该用户不存在！");
                }
                if (user.Password != dto.Password)
                {
                    throw new NotImplementedException("密码不正确！");
                }
                return user;
            }
        }

        public async Task<User> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var user = await db.Users.Include(x => x.Shop).Where(x => x.Id.ToString() == id && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("该用户不存在！");
                }

                return user;
            }
        }

        public async Task<List<User>> GetByShopIdAsync(string shopId, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Users.Where(x => x.ShopId.ToString() == shopId).ToListAsync(token);
            }
        }

        public Task<List<User>> GetAllIncludeAsync(UserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetListIncludeAsync(UserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> AddAsync(UserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync(UserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetListAsync(UserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
