using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
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
        public async Task AddAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var user = await db.Users.Where(x => x.Name == dto.Name || x.PhoneNumber == dto.PhoneNumber && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user != null)
                {
                    throw new NotImplementedException("该用户已存在！");
                }
                db.Users.Add(new User
                {
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Password = "123456",
                    RoleId = dto.RoleId,
                    RoleName = dto.RoleName
                });
                await db.SaveChangesAsync(token);
            }
        }

        public Task DeleteAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var user = await db.Users.Where(x => x.Name == dto.Name).FirstOrDefaultAsync(token);
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

        public Task<List<User>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }


        public async Task UpdateTokenAsync(UserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(dto.Id, out Guid uid))
                {
                    var user = await db.Users.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                    if (user == null)
                    {
                        throw new NotImplementedException("该用户不存在！");
                    }
                    user.RefreshToken = dto.RefreshToken;

                    await db.SaveChangesAsync(token);
                }
                else
                {
                    throw new NotImplementedException();
                }
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
                var user = await db.Users.Where(x => x.OpenId == dto.OpenId).FirstOrDefaultAsync(token);
                if (user != null)
                {
                    //throw new NotImplementedException("该用户已存在！");
                }
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
    }
}
