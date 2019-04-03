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
                var user = await db.Users.Where(x => (x.Name == dto.Name || x.PhoneNumber == dto.PhoneNumber) && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user != null)
                {
                    throw new NotImplementedException("该用户已存在！");
                }
                var entity = db.Users.Add(new User
                {
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Password = "123456",
                    RoleId = dto.RoleId,
                    RoleName = role.Name,
                    State = dto.State,
                    City = dto.City,
                    Region = dto.Region,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = streetOffice.Name,
                    DepartmentValue = dto.DepartmentValue,
                    DepartmentName = Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault()
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
                var user = await db.Users.Where(x => (x.Name == dto.Name || x.PhoneNumber == dto.PhoneNumber) && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user != null)
                {
                    throw new NotImplementedException("该用户已存在！");
                }
                var entity = db.Users.Add(new User
                {
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Password = "123456",
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
                    DepartmentName = Department.GetAll().Where(x => x.Value == dto.DepartmentValue).Select(x => x.Name).FirstOrDefault()
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

        public async Task<User> GetAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var user = await db.Users.Where(x => (x.Name == dto.Name || x.PhoneNumber == dto.Name) && x.IsDeleted == false).FirstOrDefaultAsync(token);
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
                var entity = await db.Users.Where(x => (x.Name == dto.Name || x.PhoneNumber == dto.PhoneNumber) && x.IsDeleted == false && x.Id != uid).FirstOrDefaultAsync(token);
                if (entity != null)
                {
                    throw new NotImplementedException("该用户已存在！");
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
                    DepartmentName = Department.YeZhu.Name
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
    }
}
