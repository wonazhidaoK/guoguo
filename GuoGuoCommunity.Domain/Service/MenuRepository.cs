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
    public class MenuRepository : IMenuRepository
    {
        public async Task<Menu> AddAsync(MenuDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var menu = await db.Menus.Where(x => (x.Key == dto.Key || x.Name == dto.Name) && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (menu != null)
                {
                    throw new NotImplementedException("该菜单已存在！");
                }
                var entity = db.Menus.Add(new Menu
                {
                    Key = dto.Key,
                    Name = dto.Name,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);

                return entity;
            }
        }

        public async Task DeleteAsync(MenuDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("菜单Id信息不正确！");
                }
                var menu = await db.Menus.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (menu == null)
                {
                    throw new NotImplementedException("该菜单不存在！");
                }
                menu.LastOperationTime = dto.OperationTime;
                menu.LastOperationUserId = dto.OperationUserId;
                menu.DeletedTime = dto.OperationTime;
                menu.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Menu>> GetAllAsync(MenuDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Menus.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue).ToListAsync(token);
            }
        }

        public async Task<Menu> GetByIdAsync(string id, CancellationToken token = default)
        {

            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out Guid menuId))
                {
                    return await db.Menus.Where(x => x.Id == menuId).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException();
            }
        }

        public async Task<List<Menu>> GetByIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await (from x in db.Menus where ids.Contains(x.Id.ToString()) select x).ToListAsync(token);
            }
        }

        public Task UpdateAsync(MenuDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
