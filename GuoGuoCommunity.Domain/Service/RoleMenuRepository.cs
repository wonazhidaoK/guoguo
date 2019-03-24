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
    public class RoleMenuRepository : IRoleMenuRepository
    {
        public async Task<Role_Menu> AddAsync(RoleMenuDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var role = await db.Role_Menus.Where(x => x.MenuId == dto.MenuId && x.RolesId == dto.RoleId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (role != null)
                {
                    role.LastOperationTime = dto.OperationTime;
                    role.LastOperationUserId = dto.OperationUserId;
                    await db.SaveChangesAsync(token);
                    return role;
                }
                role = new Role_Menu
                {
                    MenuId = dto.MenuId,
                    RolesId = dto.RoleId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    IsDeleted = false
                };
                db.Role_Menus.Add(role);
                await db.SaveChangesAsync(token);
                return role;
            }
        }

        public async Task DeleteAsync(RoleMenuDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {

                var roleMenu = await db.Role_Menus.Where(x => x.MenuId == dto.MenuId && x.RolesId == dto.RoleId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (roleMenu == null)
                {
                    throw new NotImplementedException("该菜单权限信息不存在！");
                }
                roleMenu.LastOperationTime = dto.OperationTime;
                roleMenu.LastOperationUserId = dto.OperationUserId;
                roleMenu.DeletedTime = dto.OperationTime;
                roleMenu.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Role_Menu>> GetByRoleIdAsync(string roleId, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Role_Menus.Where(x => x.RolesId == roleId && x.IsDeleted == false).ToListAsync(token);
            }
        }

        public async Task<List<Role_Menu>> GetAllAsync(CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Role_Menus.ToListAsync(token);
            }
        }

        public Task UpdateAsync(RoleMenuDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
