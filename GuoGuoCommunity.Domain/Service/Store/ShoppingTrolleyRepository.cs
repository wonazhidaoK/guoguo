using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Enum;
using GuoGuoCommunity.Domain.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service.Store
{
    public class ShoppingTrolleyRepository : IShoppingTrolleyRepository
    {
        public async Task<ShoppingTrolley> AddAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.OwnerCertificationRecordId, out var ocrId))
                {
                    throw new NotImplementedException("业主ID无效！");
                }
                if (!Guid.TryParse(dto.ShopCommodityId, out var scId))
                {
                    throw new NotImplementedException("店铺商品ID无效！");
                }
                ShoppingTrolley model = new ShoppingTrolley();
                var st = await db.ShoppingTrolleys.Where(item => item.ShopCommodityId == scId && item.OwnerCertificationRecordId == ocrId).FirstOrDefaultAsync(token);
                if (st != null)
                {
                    st.CommodityCount += dto.CommodityCount;
                }
                else
                {
                    model = db.ShoppingTrolleys.Add(new ShoppingTrolley
                    {
                        OwnerCertificationRecordId = ocrId,
                        ShopCommodityId = scId,
                        CommodityCount = dto.CommodityCount,
                        CreateOperationTime = dto.OperationTime,
                        CreateOperationUserId = dto.OperationUserId,
                        LastOperationTime = dto.OperationTime,
                        LastOperationUserId = dto.OperationUserId
                    });
                }
                await db.SaveChangesAsync(token);
                return model;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ShopId, out var ShopId))
                {
                    throw new NotImplementedException("店铺ID无效！");
                }
                if (!Guid.TryParse(dto.OwnerCertificationRecordId, out var ocrId))
                {
                    throw new NotImplementedException("业主ID无效！");
                }
                var list = await db.ShoppingTrolleys.Where(item => item.ShopCommodity.GoodsType.ShopId == ShopId && item.OwnerCertificationRecordId == ocrId).ToListAsync(token);
                db.ShoppingTrolleys.RemoveRange(list);

                if (await db.SaveChangesAsync(token) <= 0)
                {
                    throw new NotImplementedException("数据操作失败。");
                }

            }
        }

        /// <summary>
        /// 根据用户在店铺的购物车商品列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<ShoppingTrolley>> GetAllAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ShopId, out var ShopId))
                {
                    throw new NotImplementedException("店铺ID无效！");
                }
                if (!Guid.TryParse(dto.OwnerCertificationRecordId, out var ocrId))
                {
                    throw new NotImplementedException("业主ID无效！");
                }
                return await db.ShoppingTrolleys.Where(item => item.ShopCommodity.GoodsType.ShopId == ShopId && item.OwnerCertificationRecordId == ocrId && item.ShopCommodity.IsDeleted == false&& item.ShopCommodity.SalesTypeValue== SalesType.Shelf.Value).ToListAsync(token);
            }
        }

        /// <summary>
        /// 更新购物车
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.OwnerCertificationRecordId, out var ocrId))
                {
                    throw new NotImplementedException("业主ID无效！");
                }
                if (!Guid.TryParse(dto.ShopCommodityId, out var scId))
                {
                    throw new NotImplementedException("店铺商品ID无效！");
                }
                var model = await db.ShoppingTrolleys.Where(item => item.OwnerCertificationRecordId == ocrId && item.ShopCommodityId == scId).FirstOrDefaultAsync(token);
                if (model == null)
                {
                    throw new NotImplementedException("购物车商品不存在！");
                }

                if (model.CommodityCount > dto.CommodityCount)
                {
                    model.CommodityCount -= dto.CommodityCount;

                }
                else
                {
                    db.ShoppingTrolleys.Remove(model);
                }

                if (await db.SaveChangesAsync(token) <= 0)
                {
                    throw new NotImplementedException("数据操作失败！");
                }
            }
        }

        public Task<ShoppingTrolley> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShoppingTrolley>> GetListAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShoppingTrolley>> GetAllIncludeAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ShopId, out var ShopId))
                {
                    throw new NotImplementedException("店铺ID无效！");
                }
                if (!Guid.TryParse(dto.OwnerCertificationRecordId, out var ocrId))
                {
                    throw new NotImplementedException("业主ID无效！");
                }
                return await db.ShoppingTrolleys.Include(x => x.ShopCommodity.GoodsType.Shop).Where(item => item.ShopCommodity.GoodsType.ShopId == ShopId && item.OwnerCertificationRecordId == ocrId && item.ShopCommodity.IsDeleted == false && item.ShopCommodity.SalesTypeValue == SalesType.Shelf.Value).ToListAsync(token);
            }
        }

        public Task<ShoppingTrolley> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShoppingTrolley>> GetListIncludeAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingTrolley> GetForShopCommodityIdAsync(ShoppingTrolleyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.ShoppingTrolleys.Where(item => item.ShopCommodityId.ToString() == dto.ShopCommodityId && item.OwnerCertificationRecordId.ToString() == dto.OwnerCertificationRecordId).FirstOrDefaultAsync(token);
            }
        }
    }
}
