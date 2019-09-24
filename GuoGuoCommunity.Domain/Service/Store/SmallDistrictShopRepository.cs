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
    public class SmallDistrictShopRepository : ISmallDistrictShopRepository
    {
        public async Task<SmallDistrictShop> AddAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ShopId, out var shopId))
                {
                    throw new NotImplementedException("商店id信息不正确！");
                }
                var shop = await db.Shops.Where(x => x.Id == shopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shop == null)
                {
                    throw new NotImplementedException("该商家不存在！");
                }

                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区id信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("该小区不存在！");
                }
                if (((await db.SmallDistrictShops.Where(x => x.SmallDistrictId.ToString() == dto.SmallDistrictId && x.ShopId.ToString() == dto.ShopId && x.IsDeleted == false).ToListAsync(token)).Any()))
                {
                    throw new NotImplementedException("该商户信息已存在！");
                }
                var entity = db.SmallDistrictShops.Add(new SmallDistrictShop
                {
                    ShopId = shop.Id,
                    SmallDistrictId = smallDistrict.Id,
                    Sort = dto.Sort,
                    Postage = dto.Postage,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商户Id信息不正确！");
                }
                var shopCommodity = await db.SmallDistrictShops.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shopCommodity == null)
                {
                    throw new NotImplementedException("商户不存在！");
                }

                shopCommodity.LastOperationTime = dto.OperationTime;
                shopCommodity.LastOperationUserId = dto.OperationUserId;
                shopCommodity.DeletedTime = dto.OperationTime;
                shopCommodity.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<SmallDistrictShop>> GetAllAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SmallDistrictShop>> GetAllIncludeAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.SmallDistrictShops.Include(x => x.Shop).Where(item => item.IsDeleted == false && item.SmallDistrictId.ToString() == dto.SmallDistrictId && item.Shop.IsDeleted == false);

                list = list.OrderBy(item => item.Sort);
                // List<SmallDistrictShop> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                // SmallDistrictShopForPageDto pagelist = new SmallDistrictShopForPageDto { List = resultList, Count = list.Count() };
                return await list.ToListAsync(token);
            }
        }

        public async Task<SmallDistrictShopForPageDto> GetAllIncludeForPageAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.SmallDistrictShops.Include(x => x.Shop).Where(item => item.IsDeleted == false && item.SmallDistrictId.ToString() == dto.SmallDistrictId && item.Shop.IsDeleted == false);

                if (!string.IsNullOrEmpty(dto.ShopId))
                {
                    list = list.Where(item => item.ShopId.ToString() == dto.ShopId);
                }
                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<SmallDistrictShop> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                SmallDistrictShopForPageDto pagelist = new SmallDistrictShopForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public async Task<SmallDistrictShopForPageDto> GetAllIncludeForShopUserAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.SmallDistrictShops.Include(x => x.Shop).Where(item => item.IsDeleted == false && item.SmallDistrictId.ToString() == dto.SmallDistrictId && item.Shop.IsDeleted == false);

                list = list.OrderBy(item => item.Sort);
                List<SmallDistrictShop> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                SmallDistrictShopForPageDto pagelist = new SmallDistrictShopForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public Task<SmallDistrictShop> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<SmallDistrictShop> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.SmallDistrictShops.Include(x => x.Shop).Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该商户Id信息不正确！");
            }
        }

        //public async Task<SmallDistrictShop> GetIncludeForShopUserAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        //{
        //    using (var db = new GuoGuoCommunityContext())
        //    {
        //        //if (Guid.TryParse(id, out var uid))
        //        //{
        //        return await db.SmallDistrictShops.Where(x => x.ShopId.ToString() == dto.ShopId).FirstOrDefaultAsync(token);
        //        //}
        //        // throw new NotImplementedException("该商户Id信息不正确！");
        //    }
        //}

        public async Task<List<SmallDistrictShop>> GetListAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.SmallDistrictShops.Where(item => item.IsDeleted == false && item.SmallDistrictId.ToString() == dto.SmallDistrictId);

                return await list.ToListAsync(token);
            }
        }

        public Task<List<SmallDistrictShop>> GetListIncludeAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SmallDistrictShopDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商户信息不正确！");
                }
                var smallDistrictShop = await db.SmallDistrictShops.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (smallDistrictShop == null)
                {
                    throw new NotImplementedException("该商户不存在！");
                }

                smallDistrictShop.Postage = dto.Postage;
                smallDistrictShop.Sort = dto.Sort;
                smallDistrictShop.LastOperationTime = dto.OperationTime;
                smallDistrictShop.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }
    }
}
