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
    public class RoleService : IRoleService
    {
        public async Task AddAsync(RoleDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var role = await db.User_Roles.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();
                if (role != null)
                {
                    throw new NotImplementedException("该角色名已存在！");
                }
                db.User_Roles.Add(new User_Role
                {
                    Name = dto.Name
                });
                await db.SaveChangesAsync(token);
            }
        }

        public Task DeleteAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User_Role>> GetAllAsync(CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.User_Roles.ToListAsync(token);
            }
        }

        public Task UpdateAsync(RoleDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
