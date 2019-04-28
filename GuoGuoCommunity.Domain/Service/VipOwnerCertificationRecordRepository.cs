using EntityFramework.Extensions;
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
    public class VipOwnerCertificationRecordRepository : IVipOwnerCertificationRecordRepository
    {
        public async Task<VipOwnerCertificationRecord> AddAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.VipOwnerId, out var vipOwnerId))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwners = await db.VipOwners.Where(x => x.Id == vipOwnerId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwners == null)
                {
                    throw new NotImplementedException("业委会信息不存在！");
                }

                if (!Guid.TryParse(dto.VipOwnerStructureId, out var vipOwnerStructureId))
                {
                    throw new NotImplementedException("职能Id信息不正确！");
                }
                var vipOwnerStructure = await db.VipOwnerStructures.Where(x => x.Id == vipOwnerStructureId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwnerStructure == null)
                {
                    throw new NotImplementedException("职能信息不存在！");
                }

                if (!Guid.TryParse(dto.UserId, out var userId))
                {
                    throw new NotImplementedException("用户Id信息不正确！");
                }
                var user = await db.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("用户信息不存在！");
                }

                var entity = db.VipOwnerCertificationRecords.Add(new VipOwnerCertificationRecord
                {
                    UserId = dto.UserId,
                    VipOwnerStructureId = dto.VipOwnerStructureId,
                    VipOwnerId = dto.VipOwnerId,
                    VipOwnerName = vipOwners.Name,
                    VipOwnerStructureName = vipOwnerStructure.Name,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    OwnerCertificationId = dto.OwnerCertificationId,
                    VoteId = dto.VoteId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VipOwnerCertificationRecord>> GetAllForPropertyAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwnerCertificationRecords.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (string.IsNullOrWhiteSpace(dto.VipOwnerId))
                {
                    var vipOwner = await db.VipOwners.Where(x => x.SmallDistrictId == dto.OperationUserSmallDistrictId && x.IsDeleted == false).Select(x => x.Id.ToString()).ToListAsync(token);
                    list = list.Where(x => vipOwner.Contains(x.VipOwnerId)).ToList();
                }
                else
                {
                    list = list.Where(x => x.VipOwnerId == dto.VipOwnerId).ToList();
                }
                list = list.Where(x => x.CreateOperationTime >= dto.StartTime && x.CreateOperationTime <= dto.EndTime).ToList();
                return list;
            }

        }

        public Task<VipOwnerCertificationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VipOwnerCertificationRecord>> GetListAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationRecords.Where(x => x.IsDeleted == false && x.UserId == dto.UserId).ToListAsync(token);
            }
        }

        public Task UpdateAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        #region 事件

        public void OnSubscribe(VipOwnerIncrementer incrementer)
        {
            incrementer.VipOwnerEvent += VipOwnerChanging;//在发布者私有委托里增加方法
        }

        public async void VipOwnerChanging(GuoGuoCommunityContext dbs, VipOwner vipOwner, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.VipOwnerCertificationRecords.Where(x => x.VipOwnerId == vipOwner.Id.ToString()).UpdateAsync(x => new VipOwnerCertificationRecord { VipOwnerName = vipOwner.Name });
            }
        }

        public void OnSubscribe(VipOwnerStructureIncrementer incrementer)
        {
            incrementer.VipOwnerStructureEvent += VipOwnerStructureChanging;//在发布者私有委托里增加方法
        }

        public async void VipOwnerStructureChanging(GuoGuoCommunityContext dbs, VipOwnerStructure vipOwnerStructure, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.VipOwnerCertificationRecords.Where(x => x.VipOwnerStructureId == vipOwnerStructure.Id.ToString()).UpdateAsync(x => new VipOwnerCertificationRecord { VipOwnerStructureName = vipOwnerStructure.Name });
            }
        }

        #endregion

        public async Task<VipOwnerCertificationRecord> GetForVipOwnerIdAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationRecords.Where(x => x.IsDeleted == false && x.UserId == dto.UserId && x.VipOwnerId == dto.VipOwnerId).FirstOrDefaultAsync(token);
            }
        }

        public async Task<List<VipOwnerCertificationRecord>> GetForVipOwnerIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationRecords.Where(x => x.IsDeleted == false && ids.Contains(x.VipOwnerId.ToString())).ToListAsync(token);
            }
        }
    }
}
