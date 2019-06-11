using EntityFramework.Extensions;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class ShopUserAddressRepository : IShopUserAddressRepository
    {
        public async Task<ShopUserAddress> AddAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ApplicationRecordId, out var applicationRecordId))
                {
                    throw new NotImplementedException("业主申请Id信息不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == applicationRecordId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主申请记录不存在！");
                }

                if (!Guid.TryParse(dto.IndustryId, out var industryId))
                {
                    throw new NotImplementedException("业户Id信息不正确！");
                }
                var industry = await db.Industries.Where(x => x.Id == industryId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (industry == null)
                {
                    throw new NotImplementedException("业户信息不存在！");
                }

                if (dto.IsDefault)
                {
                    await db.ShopUserAddresses.Where(x => x.ApplicationRecordId == applicationRecordId || x.IsDefault).UpdateAsync(x => new ShopUserAddress { IsDefault = false });
                }

                var entity = db.ShopUserAddresses.Add(new ShopUserAddress
                {
                    IndustryId = industry.Id,
                    ApplicationRecordId = ownerCertificationRecord.Id,
                    IsDefault = dto.IsDefault,
                    ReceiverName = dto.ReceiverName,
                    ReceiverPhone = dto.ReceiverPhone,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("用户地址Id信息不正确！");
                }
                var shopUserAddress = await db.ShopUserAddresses.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (shopUserAddress == null)
                {
                    throw new NotImplementedException("用户地址不存在！");
                }

                shopUserAddress.LastOperationTime = dto.OperationTime;
                shopUserAddress.LastOperationUserId = dto.OperationUserId;
                shopUserAddress.DeletedTime = dto.OperationTime;
                shopUserAddress.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<ShopUserAddress>> GetAllAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShopUserAddress>> GetAllIncludeAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.ShopUserAddresses.Include(x => x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice).Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.ApplicationRecordId))
                {
                    list = list.Where(x => x.ApplicationRecordId.ToString() == dto.ApplicationRecordId).ToList();
                }

                return list;
            }
        }

        public Task<ShopUserAddressForPageDto> GetAllIncludeForPageAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<ShopUserAddress> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ShopUserAddress> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var ID))
                {
                    throw new NotImplementedException("地址ID无效！");
                }
                return await db.ShopUserAddresses.Include(x => x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice).Where(item => item.Id == ID).FirstOrDefaultAsync(token);
            }
        }

        public Task<List<ShopUserAddress>> GetListAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShopUserAddress>> GetListIncludeAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.ShopUserAddresses.Include(x => x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice).Where(item => item.IsDeleted == false && item.ApplicationRecordId.ToString() == dto.ApplicationRecordId);

                return await list.ToListAsync(token);
            }
        }

        public async Task UpdateAsync(ShopUserAddressDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var ID))
                {
                    throw new NotImplementedException("地址ID无效！");
                }
                var shopUserAddress = await db.ShopUserAddresses.Where(item => item.Id == ID).FirstOrDefaultAsync(token);
                if (shopUserAddress == null)
                {
                    throw new NotImplementedException("地址不存在！");
                }

                if (!Guid.TryParse(dto.IndustryId, out var industryId))
                {
                    throw new NotImplementedException("业户Id信息不正确！");
                }
                var industry = await db.Industries.Where(x => x.Id == industryId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (industry == null)
                {
                    throw new NotImplementedException("业户信息不存在！");
                }

                if (dto.IsDefault)
                {
                    await db.ShopUserAddresses.Where(x => x.ApplicationRecordId == shopUserAddress.ApplicationRecordId && x.IsDefault && x.Id != shopUserAddress.Id).UpdateAsync(x => new ShopUserAddress { IsDefault = false });
                }

                shopUserAddress.IsDefault = dto.IsDefault;
                shopUserAddress.IndustryId = industry.Id;
                shopUserAddress.ReceiverPhone = dto.ReceiverPhone;
                shopUserAddress.ReceiverName = dto.ReceiverName;
                shopUserAddress.LastOperationTime = dto.OperationTime;
                shopUserAddress.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }
    }
}
