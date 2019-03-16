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

        public async Task AddAsync(MenuDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var menu = await db.Menus.Where(x => x.Kay == dto.Kay || x.Name == dto.Name).FirstOrDefaultAsync(token);
                if (menu != null)
                {
                    throw new NotImplementedException("该菜单已存在！");
                }
                db.Menus.Add(new Menu
                {
                    Kay = dto.Kay,
                    Name = dto.Name
                });
                await db.SaveChangesAsync(token);
            }
        }

        public Task DeleteAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Menu>> GetAllAsync(CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Menus.ToListAsync(token);
            }
        }

        public async Task<Menu> GetByIdAsync(string id, CancellationToken token = default)
        {
            
            using (var db = new GuoGuoCommunityContext())
            {
                if(Guid.TryParse(id,out Guid menuId))
                {
                    return await db.Menus.Where(x=>x.Id== menuId).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException();
            }
        }

        public Task UpdateAsync(MenuDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
