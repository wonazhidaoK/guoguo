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
    public class PlatformCommodityRepository : IPlatformCommodityRepository
    {
        public async Task<PlatformCommodity> AddAsync(PlatformCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var platformCommodity = await db.PlatformCommodities.Where(x => x.BarCode == dto.BarCode && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (platformCommodity != null)
                {
                    throw new NotImplementedException("该商品信息已存在！");
                }
                var entity = db.PlatformCommodities.Add(new PlatformCommodity
                {
                    Name = dto.Name,
                    BarCode = dto.BarCode,
                    ImageUrl = dto.ImageUrl,
                    Price = dto.Price,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        /// <summary>
        /// 删除平台商品
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeleteAsync(PlatformCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var ID))
                {
                    throw new NotImplementedException("平台商品ID无效！");
                }
                var platformCommoditie = await db.PlatformCommodities.Where(item => item.Id == ID && item.IsDeleted == false).FirstOrDefaultAsync(token);
                if (platformCommoditie == null)
                {
                    throw new NotImplementedException("该平台商品不存在！");
                }
                platformCommoditie.LastOperationTime = dto.OperationTime;
                platformCommoditie.LastOperationUserId = dto.OperationUserId;
                platformCommoditie.DeletedTime = dto.OperationTime;
                platformCommoditie.IsDeleted = true;
                int result = await db.SaveChangesAsync(token);
                if (result <= 0)
                {
                    throw new NotImplementedException("数据执行失败。");
                }
            }
        }

        /// <summary>
        /// 查询所有平台商品
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PlatformCommodityForPage> GetListForPageAsync(PlatformCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.PlatformCommodities.Where(item => item.IsDeleted == false);
                if (!string.IsNullOrEmpty(dto.Name))
                {
                    list = list.Where(item => item.Name.Contains(dto.Name));
                }
                if (!string.IsNullOrEmpty(dto.BarCode))
                {
                    list = list.Where(item => item.BarCode == dto.BarCode);
                }
                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<PlatformCommodity> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                PlatformCommodityForPage pagelist = new PlatformCommodityForPage { PlatformCommoditieForPageList = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        /// <summary>
        /// 根据ID查询平台商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PlatformCommodity> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var ID))
                {
                    throw new NotImplementedException("平台商品ID无效！");
                }
                return await db.PlatformCommodities.Where(item => item.Id == ID).FirstOrDefaultAsync(token);
            }
        }

        /// <summary>
        /// 更新平台商品
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task UpdateAsync(PlatformCommodityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var ID))
                {
                    throw new NotImplementedException("平台商品ID无效！");
                }
                var model = await db.PlatformCommodities.Where(item => item.Id == ID).FirstOrDefaultAsync(token);
                if (model == null)
                {
                    throw new NotImplementedException("平台商品不存在！");
                }
                if (db.PlatformCommodities.Any(item => item.BarCode == dto.BarCode && item.IsDeleted == false && item.Id != ID))
                {
                    throw new NotImplementedException("当前条码已经存在！");
                }
                model.Name = dto.Name;
                model.Price = dto.Price;
                model.ImageUrl = dto.ImageUrl;
                model.LastOperationTime = dto.OperationTime;
                model.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }

        public Task<List<PlatformCommodity>> GetListAsync(PlatformCommodityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlatformCommodity>> GetAllAsync(PlatformCommodityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PlatformCommodity> GetForBarCodeAsync(string barCode, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.PlatformCommodities.Where(item => item.BarCode == barCode).FirstOrDefaultAsync(token);
            }
        }
    }
}
