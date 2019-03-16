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
    public class OwnerCertificationRecordRepository : IOwnerCertificationRecordRepository
    {
        public async Task<OwnerCertificationRecord> AddAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.UserId, out var userId))
                {
                    throw new NotImplementedException("用户Id不正确！");
                }
                var user = await db.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("用户信息不存在！");
                }

                if (!Guid.TryParse(dto.OwnerId, out var ownerId))
                {
                    throw new NotImplementedException("业主Id不正确！");
                }
                var owner = await db.Owners.Where(x => x.Id == ownerId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (owner == null)
                {
                    throw new NotImplementedException("业主信息不存在！");
                }

                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.UserId == dto.UserId&&x.OwnerId==dto.OwnerId&&x.CertificationStatusValue!= OwnerCertification.Failure.Value && x.IsDeleted == false && x.OwnerId == dto.OwnerId).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord != null)
                {
                    throw new NotImplementedException("该业主信息已存在！");
                }
                var entity = db.OwnerCertificationRecords.Add(new OwnerCertificationRecord
                {
                    CertificationResult = dto.CertificationResult,
                    UserId = dto.UserId,
                    OwnerId = dto.OwnerId,
                    CertificationStatusName = OwnerCertification.Executing.Name,
                    CertificationStatusValue = OwnerCertification.Executing.Value,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("认证Id信息不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("该业主信息不存在！");
                }

                if (OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该业主信息下存在下级数据");
                }

                ownerCertificationRecord.LastOperationTime = dto.OperationTime;
                ownerCertificationRecord.LastOperationUserId = dto.OperationUserId;
                ownerCertificationRecord.DeletedTime = dto.OperationTime;
                ownerCertificationRecord.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<OwnerCertificationRecord>> GetAllAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<OwnerCertificationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OwnerCertificationRecord>> GetListAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.UserId == dto.UserId).ToListAsync(token);
            }
        }



        public async Task UpdateAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("认证信息Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("该认证信息不存在！");
                }

                ownerCertificationRecord.CertificationStatusName = dto.CertificationStatusName;
                ownerCertificationRecord.CertificationStatusValue = dto.CertificationStatusValue;
                ownerCertificationRecord.LastOperationTime = dto.OperationTime;
                ownerCertificationRecord.LastOperationUserId = dto.OperationUserId;
                OnUpdate(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        private void OnUpdate(GuoGuoCommunityContext db, OwnerCertificationRecordDto dto, CancellationToken token = default)
        {

        }

        private bool OnDelete(GuoGuoCommunityContext db, OwnerCertificationRecordDto dto, CancellationToken token = default)
        {

            return false;
        }

        public Task UpdateStatusAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OwnerCertificationRecord>> GetListForOwnerAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.OwnerId == dto.OwnerId).ToListAsync(token);
            }
        }
    }
}
