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
    public class GoodsTypeRepository : IGoodsTypeRepository
    {
        public async Task<GoodsType> AddAsync(GoodsTypeDto dto, CancellationToken token = default)
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
                var goodsType = await db.GoodsTypes.Where(x => x.Name == dto.Name && x.ShopId.ToString() == dto.ShopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (goodsType != null)
                {
                    throw new NotImplementedException("该商品类别已存在！");
                }
                var entity = db.GoodsTypes.Add(new GoodsType
                {
                    ShopId = shop.Id,
                    Name = dto.Name,
                    Sort = dto.Sort,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("商品分类Id信息不正确！");
                }
                var goodsType = await db.GoodsTypes.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (goodsType == null)
                {
                    throw new NotImplementedException("该商品分类不存在！");
                }
                if (goodsType.ShopId.ToString() != dto.ShopId)
                {
                    throw new NotImplementedException("该商品分类不属于该商铺！");
                }
                if (await OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该店铺商品分类下存在下级业务数据");
                }

                goodsType.LastOperationTime = dto.OperationTime;
                goodsType.LastOperationUserId = dto.OperationUserId;
                goodsType.DeletedTime = dto.OperationTime;
                goodsType.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<GoodsType>> GetAllAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<GoodsType>> GetAllIncludeAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<GoodsTypeForPageDto> GetAllIncludeForPageAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.GoodsTypes.Where(item => item.IsDeleted == false && item.ShopId.ToString() == dto.ShopId);
                if (!string.IsNullOrEmpty(dto.Name))
                {
                    list = list.Where(item => item.Name.Contains(dto.Name));
                }

                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<GoodsType> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                GoodsTypeForPageDto pagelist = new GoodsTypeForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public Task<GoodsType> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<GoodsType> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GoodsType>> GetListAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.GoodsTypes.Where(item => item.IsDeleted == false && item.ShopId.ToString() == dto.ShopId).OrderBy(item => item.Sort);

                return await list.ToListAsync(token);
            }
        }

        public Task<List<GoodsType>> GetListIncludeAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(GoodsTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var ID))
                {
                    throw new NotImplementedException("商品分类ID无效！");
                }
                var goodsType = await db.GoodsTypes.Where(item => item.Id == ID).FirstOrDefaultAsync(token);
                if (goodsType == null)
                {
                    throw new NotImplementedException("商品分类不存在！");
                }

                if (goodsType.ShopId.ToString() != dto.ShopId)
                {
                    throw new NotImplementedException("该商品分类不属于该商铺！");
                }

                if ((await db.GoodsTypes.Where(x => x.Name == dto.Name && x.ShopId.ToString() == dto.ShopId && x.IsDeleted == false && x.Id != ID).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该商品类别已存在！");
                }
                goodsType.Name = dto.Name;
                goodsType.Sort = dto.Sort;
                goodsType.LastOperationTime = dto.OperationTime;
                goodsType.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }

        private async Task<bool> OnDelete(GuoGuoCommunityContext db, GoodsTypeDto dto, CancellationToken token = default)
        {
            //店铺商品
            if (await db.ShopCommodities.Where(x => x.TypeId.ToString() == dto.Id && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }
            return false;
        }
    }
}
