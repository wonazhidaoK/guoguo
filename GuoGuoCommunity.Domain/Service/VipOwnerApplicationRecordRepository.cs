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
    public class VipOwnerApplicationRecordRepository : IVipOwnerApplicationRecordRepository
    {
        public async Task<VipOwnerApplicationRecord> AddAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.UserId, out var userId))
                {
                    throw new NotImplementedException("用户Id信息不正确！");
                }
                var user = await db.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("用户信息不存在！");
                }
                if (!Guid.TryParse(dto.StructureId, out var structureId))
                {
                    throw new NotImplementedException("申请职能Id信息不正确！");
                }
                var vipOwnerStructure = await db.VipOwnerStructures.Where(x => x.Id == structureId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwnerStructure == null)
                {
                    throw new NotImplementedException("申请职能不存在！");
                }

                var vipOwnerApplicationRecord = await db.VipOwnerApplicationRecords.Where(x => x.UserId == dto.UserId && (x.IsDeleted == false || x.IsInvalid == false)).FirstOrDefaultAsync(token);
                if (vipOwnerApplicationRecord != null)
                {
                    throw new NotImplementedException("存在职能申请！");
                }

                var entity = db.VipOwnerApplicationRecords.Add(new VipOwnerApplicationRecord
                {
                    Reason = dto.Reason,
                    StructureId = dto.StructureId,
                    StructureName = dto.StructureName,
                    UserId = dto.UserId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerApplicationRecord>> GetAllAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerApplicationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VipOwnerApplicationRecord>> GetListAsync(List<string> dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.IsInvalid == false && dto.Contains(x.UserId)).ToListAsync(token);
            }
        }

        public async Task<bool> IsPresenceforUserId(string userId, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.UserId == userId && x.IsDeleted == false && x.IsInvalid == false).AnyAsync(token);
            }
        }

        public Task UpdateAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
