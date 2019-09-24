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
    public class ShopRepository : IShopRepository
    {
        public async Task<Shop> AddAsync(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var shop = await db.Shops.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shop != null)
                {
                    throw new NotImplementedException("该商户信息已存在！");
                }
                var entity = db.Shops.Add(new Shop
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Description = dto.Description,
                    LogoImageUrl = dto.LogoImageUrl,
                    MerchantCategoryName = dto.MerchantCategoryName,
                    MerchantCategoryValue = dto.MerchantCategoryValue,
                    QualificationImageUrl = dto.QualificationImageUrl,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    PhoneNumber = dto.PhoneNumber,
                    PrinterName = dto.PrinterName
                    //UserId = user.Id
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商户信息不正确！");
                }
                var shop = await db.Shops.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shop == null)
                {
                    throw new NotImplementedException("商户信息不存在！");
                }
                if (await OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该商户存在下级社区数据！");
                }
                shop.LastOperationTime = dto.OperationTime;
                shop.LastOperationUserId = dto.OperationUserId;
                shop.DeletedTime = dto.OperationTime;
                shop.IsDeleted = true;

                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Shop>> GetAllAsync(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Shops.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }

                return list;
            }
        }

        public async Task<List<Shop>> GetAllIncludeAsync(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Shops.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.MerchantCategoryValue))
                {
                    list = list.Where(x => x.MerchantCategoryValue == dto.MerchantCategoryValue).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                {
                    list = list.Where(x => x.PhoneNumber.Contains(dto.PhoneNumber)).ToList();
                }
                return list;
            }
        }

        public async Task<Shop> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Shops.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该商户Id信息不正确！");
            }
        }

        public async Task<Shop> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Shops.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该商户Id信息不正确！");
            }
        }

        public async Task<List<Shop>> GetListAsync(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Shops.Where(x => x.IsDeleted == false).ToListAsync(token);

                return list;
            }
        }

        public async Task<List<Shop>> GetListForNotIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                // var list = db.GoodsTypes.Where(item => item.IsDeleted == false && item.ShopId.ToString() == dto.ShopId);
                //using (var db = new GuoGuoCommunityContext())
                //{
                return await (from x in db.Shops where !ids.Contains(x.Id.ToString()) && x.IsDeleted == false select x).ToListAsync(token);
                //}
                //return await list.ToListAsync(token);
            }
        }

        public Task<List<Shop>> GetListIncludeAsync(ShopDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商户信息不正确！");
                }
                var shop = await db.Shops.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (shop == null)
                {
                    throw new NotImplementedException("该商户不存在！");
                }
                if (((await db.Shops.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.Id != uid).ToListAsync(token)).Any()))
                {
                    throw new NotImplementedException("该商户信息已存在！");
                }
               
                shop.LogoImageUrl = dto.LogoImageUrl;
                shop.QualificationImageUrl = dto.QualificationImageUrl;
                shop.Address = dto.Address;
                shop.Description = dto.Description;
                shop.MerchantCategoryValue = dto.MerchantCategoryValue;
                shop.MerchantCategoryName = dto.MerchantCategoryName;
                shop.PhoneNumber = dto.PhoneNumber;
                shop.LastOperationTime = dto.OperationTime;
                shop.LastOperationUserId = dto.OperationUserId;
                shop.PrinterName = dto.PrinterName;
                shop.Name = dto.Name;
                return await db.SaveChangesAsync(token);
            }
        }

        Task IRepository<Shop, ShopDto>.UpdateAsync(ShopDto dto, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> OnDelete(GuoGuoCommunityContext db, ShopDto dto, CancellationToken token = default)
        {
            //商品分类
            if (await db.GoodsTypes.Where(x => x.ShopId.ToString() == dto.Id && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 更新店铺开启的活动
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> UpdateShopActivitySign(ShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商户信息不正确！");
                }
                var shop = await db.Shops.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (shop == null)
                {
                    throw new NotImplementedException("该商户不存在！");
                }
                shop.ActivitySign = dto.ActivitySign;
                shop.LastOperationTime = dto.OperationTime;
                shop.LastOperationUserId = dto.OperationUserId;
                return await db.SaveChangesAsync(token) > 0;
            }
        }
    }
}
