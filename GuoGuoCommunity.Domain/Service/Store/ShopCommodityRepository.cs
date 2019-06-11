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
    public class ShopCommodityRepository : IShopCommodityRepository
    {
        public async Task<ShopCommodity> AddAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.TypeId, out var typeId))
                {
                    throw new NotImplementedException("商品分类id信息不正确！");
                }
                var goodsType = await db.GoodsTypes.Where(x => x.Id == typeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (goodsType == null)
                {
                    throw new NotImplementedException("商品分类不存在！");
                }
                var shopCommodity = await db.ShopCommodities.Where(x => x.BarCode == dto.BarCode && x.GoodsType.ShopId == goodsType.ShopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shopCommodity != null)
                {
                    throw new NotImplementedException("该商品信息已存在！");
                }
                var platformCommodity = await db.PlatformCommodities.Where(x => x.BarCode == dto.BarCode && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (platformCommodity == null)
                {
                    db.PlatformCommodities.Add(new PlatformCommodity
                    {
                        BarCode = dto.BarCode,
                        ImageUrl = dto.ImageUrl,
                        Name = dto.Name,
                        Price = dto.Price,
                        CreateOperationTime = dto.OperationTime,
                        CreateOperationUserId = dto.OperationUserId
                    });
                }
                var entity = db.ShopCommodities.Add(new ShopCommodity
                {
                    TypeId = goodsType.Id,
                    ImageUrl = dto.ImageUrl,
                    Price = dto.Price,
                    BarCode = dto.BarCode,
                    CommodityStocks = dto.CommodityStocks,
                    SalesTypeValue = dto.SalesTypeValue,
                    SalesTypeName = dto.SalesTypeName,
                    Description = dto.Description,
                    DiscountPrice = dto.DiscountPrice,
                    Name = dto.Name,
                    Sort = dto.Sort,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商品Id信息不正确！");
                }
                var shopCommodity = await db.ShopCommodities.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shopCommodity == null)
                {
                    throw new NotImplementedException("该商品不存在！");
                }
                if (shopCommodity.SalesTypeValue == SalesType.Shelf.Value)
                {
                    throw new NotImplementedException("该商品为销售状态不允许删除！");
                }

                shopCommodity.LastOperationTime = dto.OperationTime;
                shopCommodity.LastOperationUserId = dto.OperationUserId;
                shopCommodity.DeletedTime = dto.OperationTime;
                shopCommodity.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<ShopCommodity>> GetAllAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShopCommodity>> GetAllIncludeAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ShopCommodityForPageDto> GetAllIncludeForPageAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.ShopCommodities.Include(x => x.GoodsType.Shop).Where(item => item.IsDeleted == false && item.GoodsType.ShopId.ToString() == dto.ShopId);
                if (!string.IsNullOrEmpty(dto.Name))
                {
                    list = list.Where(item => item.Name.Contains(dto.Name));
                }
                if (!string.IsNullOrEmpty(dto.BarCode))
                {
                    list = list.Where(item => item.BarCode.Contains(dto.BarCode));
                }
                if (!string.IsNullOrEmpty(dto.TypeId))
                {
                    list = list.Where(item => item.TypeId.ToString() == dto.TypeId);
                }
                if (!string.IsNullOrEmpty(dto.SalesTypeValue))
                {
                    list = list.Where(item => item.SalesTypeValue == dto.SalesTypeValue);
                }
                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<ShopCommodity> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                ShopCommodityForPageDto pagelist = new ShopCommodityForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public Task<ShopCommodity> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ShopCommodity> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.ShopCommodities.Include(x => x.GoodsType.Shop).Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该商品Id信息不正确！");
            }
        }

        public async Task<List<ShopCommodity>> GetListAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.ShopCommodities.Where(item => item.IsDeleted == false && item.TypeId.ToString() == dto.TypeId && item.SalesTypeValue == SalesType.Shelf.Value).OrderBy(item => item.Sort);

                return await list.ToListAsync(token);
            }
        }

        public async Task<List<ShopCommodity>> GetListIncludeAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.ShopCommodities.Where(item => item.IsDeleted == false && item.TypeId.ToString() == dto.TypeId);

                return await list.ToListAsync(token);
            }
        }

        public async Task UpdateAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.TypeId, out var typeId))
                {
                    throw new NotImplementedException("商品分类ID无效！");
                }
                var goodsType = await db.GoodsTypes.Where(item => item.Id == typeId).FirstOrDefaultAsync(token);
                if (goodsType == null)
                {
                    throw new NotImplementedException("商品分类不存在！");
                }

                if (!Guid.TryParse(dto.Id, out var Id))
                {
                    throw new NotImplementedException("商品ID无效！");
                }
                var shopCommodity = await db.ShopCommodities.Where(item => item.Id == Id).FirstOrDefaultAsync(token);
                if (shopCommodity == null)
                {
                    throw new NotImplementedException("商品不存在！");
                }

                shopCommodity.TypeId = goodsType.Id;
                shopCommodity.ImageUrl = dto.ImageUrl;
                shopCommodity.Price = dto.Price;
                shopCommodity.CommodityStocks = dto.CommodityStocks;
                shopCommodity.Description = dto.Description;
                shopCommodity.DiscountPrice = dto.DiscountPrice;
                shopCommodity.Name = dto.Name;
                shopCommodity.Sort = dto.Sort;
                shopCommodity.LastOperationTime = dto.OperationTime;
                shopCommodity.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }

        public async Task UpdateSalesTypeAsync(ShopCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var Id))
                {
                    throw new NotImplementedException("商品ID无效！");
                }
                var shopCommodity = await db.ShopCommodities.Where(item => item.Id == Id).FirstOrDefaultAsync(token);
                if (shopCommodity == null)
                {
                    throw new NotImplementedException("商品不存在！");
                }
                shopCommodity.SalesTypeName = dto.SalesTypeName;
                shopCommodity.SalesTypeValue = dto.SalesTypeValue;
                shopCommodity.LastOperationTime = dto.OperationTime;
                shopCommodity.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }
    }
}
