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
    public class VipOwnerService : IVipOwnerService
    {
        public async Task<VipOwner> AddAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.Name == dto.SmallDistrictName && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }
                var vipOwners = await db.VipOwners.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId).FirstOrDefaultAsync(token);
                if (vipOwners != null)
                {
                    throw new NotImplementedException("该业委会已存在！");
                }
                var entity = db.VipOwners.Add(new VipOwner
                {
                    Name = dto.Name,
                    RemarkName = dto.RemarkName,
                    SmallDistrictId = dto.SmallDistrictId,
                    SmallDistrictName = dto.SmallDistrictName,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwners = await db.VipOwners.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwners == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }

                vipOwners.LastOperationTime = dto.OperationTime;
                vipOwners.LastOperationUserId = dto.OperationUserId;
                vipOwners.DeletedTime = dto.OperationTime;
                vipOwners.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<VipOwner>> GetAllAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwners .Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    list = list.Where(x => x.SmallDistrictId == dto.SmallDistrictId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.RemarkName))
                {
                    list = list.Where(x => x.RemarkName.Contains(dto.RemarkName)).ToList();
                }
                return list;
            }
        }

        public async Task<VipOwner> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var uid))
                {
                    return await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该业委会信息不正确！");
            }
        }

        public async Task<List<VipOwner>> GetListAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwners.Where(x => x.IsDeleted == false).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    list = list.Where(x => x.SmallDistrictId == dto.SmallDistrictId).ToList();
                }
                return list;
            }
        }

        public async Task UpdateAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwners = await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwners == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }
               
                vipOwners.RemarkName = dto.RemarkName;
                vipOwners.LastOperationTime = dto.OperationTime;
                vipOwners.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }
    }
}
