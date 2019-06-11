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
    public class RoleRepository : IRoleRepository
    {
        public async Task<User_Role> AddAsync(RoleDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var role = await db.User_Roles.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync();
                if (role != null)
                {
                    throw new NotImplementedException("该角色名已存在！");
                }
                var entity = db.User_Roles.Add(new User_Role
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    DepartmentValue = dto.DepartmentValue,
                    DepartmentName = dto.DepartmentName,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(RoleDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("角色Id信息不正确！");
                }
                var user_Role = await db.User_Roles.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user_Role == null)
                {
                    throw new NotImplementedException("该角色不存在！");
                }
                user_Role.LastOperationTime = dto.OperationTime;
                user_Role.LastOperationUserId = dto.OperationUserId;
                user_Role.DeletedTime = dto.OperationTime;
                user_Role.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<User_Role>> GetAllAsync(RoleDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.User_Roles.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.DepartmentValue))
                {
                    list = list.Where(x => x.DepartmentValue == dto.DepartmentValue).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                return list;
            }
        }

        public async Task<User_Role> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.User_Roles.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该角色Id信息不正确！");
            }
        }

        public Task UpdateAsync(RoleDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
