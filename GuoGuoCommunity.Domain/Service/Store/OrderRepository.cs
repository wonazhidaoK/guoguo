using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service.Store
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<Order> AddAsync(OrderDto dto, CancellationToken token = default)
        {
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    if (!Guid.TryParse(dto.ShopId, out var shopId))
                    {
                        throw new NotImplementedException("商店id信息不正确！");
                    }

                    var smallDistrictShop = await db.SmallDistrictShops.Include(x => x.Shop).Where(x => x.Id == shopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                    if (smallDistrictShop == null)
                    {
                        throw new NotImplementedException("该商家不存在！");
                    }
                    if (!Guid.TryParse(dto.OwnerCertificationRecordId, out var ownerCertificationRecordId))
                    {
                        throw new NotImplementedException("业主认证Id信息不正确！");
                    }
                    var list = await db.ShoppingTrolleys.Include(x => x.ShopCommodity.GoodsType.Shop).Where(item => item.ShopCommodity.GoodsType.ShopId == smallDistrictShop.ShopId && item.OwnerCertificationRecordId == ownerCertificationRecordId && item.ShopCommodity.IsDeleted == false).ToListAsync(token);

                    using (var transaction = db.Database.BeginTransaction())
                    {

                        if (!list.Any())
                        {
                            throw new NotImplementedException("购物车内无商品！");
                        }
                        var price = list.Sum(x => x.CommodityCount * x.ShopCommodity.DiscountPrice);
                        var entity = db.Orders.Add(new Order
                        {
                            ShopId = smallDistrictShop.ShopId,
                            Address = dto.Address,
                            DeliveryName = dto.DeliveryName,
                            DeliveryPhone = dto.DeliveryPhone,
                            Number = dto.Number,
                            OrderStatusValue = dto.OrderStatusValue,
                            OrderStatusName = dto.OrderStatusName,
                            ReceiverName = dto.ReceiverName,
                            ReceiverPhone = dto.ReceiverPhone,
                            OwnerCertificationRecordId = ownerCertificationRecordId,
                            LastOperationTime = dto.OperationTime,
                            LastOperationUserId = dto.OperationUserId,
                            CreateOperationTime = dto.OperationTime,
                            CreateOperationUserId = dto.OperationUserId,
                            ShopCommodityCount = list.Sum(x => x.CommodityCount),
                            ShopCommodityPrice = price,
                            Postage = smallDistrictShop.Postage,
                            PaymentPrice = price + smallDistrictShop.Postage
                        });
                        db.SaveChanges();
                        foreach (var item in list)
                        {
                            db.OrdeItems.Add(new OrderItem
                            {
                                CommodityCount = item.CommodityCount,
                                DiscountPrice = item.ShopCommodity.DiscountPrice,
                                ShopCommodityId = item.ShopCommodityId,
                                Price = item.ShopCommodity.Price,
                                OrderId = entity.Id,
                                ImageUrl = item.ShopCommodity.ImageUrl,
                                Name = item.ShopCommodity.Name
                            });
                        }
                        db.SaveChanges();
                        db.ShoppingTrolleys.RemoveRange(list);

                        db.SaveChanges();

                        //提交事务
                        transaction.Commit();
                        return entity;
                    }
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }

        public async Task DeleteAsync(OrderDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("订单Id信息不正确！");
                }
                var order = await db.Orders.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (order == null)
                {
                    throw new NotImplementedException("该订单信息不存在！");
                }
                if (order.OrderStatusValue != OrderStatus.Finish.Value)
                {
                    throw new NotImplementedException("订单未完成不能删除！");
                }


                order.LastOperationTime = dto.OperationTime;
                order.LastOperationUserId = dto.OperationUserId;
                order.DeletedTime = dto.OperationTime;
                order.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<Order>> GetAllAsync(OrderDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllIncludeAsync(OrderDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderForPageDto> GetAllIncludeForMerchantAsync(OrderDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.Orders.Include(x => x.Shop).Where(item =>  item.ShopId.ToString() == dto.ShopId);
                if (!string.IsNullOrEmpty(dto.OrderStatusValue))
                {

                    list = list.Where(item => item.OrderStatusValue == dto.OrderStatusValue);
                }
                if (!string.IsNullOrEmpty(dto.Number))
                {
                    list = list.Where(item => item.Number.Contains(dto.Number));
                }
                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<Order> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                OrderForPageDto pagelist = new OrderForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public async Task<OrderForPageDto> GetAllIncludeForPageAsync(OrderDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.Orders.Include(x => x.Shop).Where(item => item.IsDeleted == false && item.OwnerCertificationRecordId.ToString() == dto.OwnerCertificationRecordId);
                if (!string.IsNullOrEmpty(dto.OrderStatusValue))
                {
                    if (dto.OrderStatusValue == "All")
                    {

                    }
                    else if (dto.OrderStatusValue == "WaitingTake")
                    {
                        list = list.Where(item => item.OrderStatusValue == "WaitingTake" || item.OrderStatusValue == "WaitingSend");
                    }
                    else
                    {
                        list = list.Where(item => item.OrderStatusValue == dto.OrderStatusValue);
                    }
                }

                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<Order> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                OrderForPageDto pagelist = new OrderForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public async Task<OrderForPageDto> GetAllIncludeForPropertyAsync(OrderDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.Orders.Include(x => x.Shop).Where(item => item.OwnerCertificationRecord.Industry.BuildingUnit.Building.SmallDistrictId.ToString() == dto.SmallDistrictId && (item.OrderStatusValue != OrderStatus.WaitingAccept.Value));
                if (!string.IsNullOrEmpty(dto.OrderStatusValue))
                {
                    list = list.Where(item => item.OrderStatusValue == dto.OrderStatusValue);
                }
                if (!string.IsNullOrEmpty(dto.Number))
                {
                    list = list.Where(item => item.Number.Contains(dto.Number));
                }
                if (!string.IsNullOrEmpty(dto.ShopId))
                {
                    list = list.Where(item => item.ShopId.ToString() == dto.ShopId);
                }
                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<Order> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                OrderForPageDto pagelist = new OrderForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public Task<Order> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var ID))
                {
                    throw new NotImplementedException("订单ID无效！");
                }
                return await db.Orders.Include(x => x.Shop).Include(x => x.OwnerCertificationRecord.Owner.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice).Where(item => item.Id == ID).FirstOrDefaultAsync(token);
            }
        }

        public Task<List<Order>> GetListAsync(OrderDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetListIncludeAsync(OrderDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(OrderDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var ID))
                {
                    throw new NotImplementedException("订单ID无效！");
                }
                var order = await db.Orders.Where(item => item.Id == ID).FirstOrDefaultAsync(token);
                if (order == null)
                {
                    throw new NotImplementedException("订单不存在！");
                }

                order.OrderStatusName = dto.OrderStatusName;
                order.OrderStatusValue = dto.OrderStatusValue;
                order.LastOperationTime = dto.OperationTime;
                order.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }
    }
}
