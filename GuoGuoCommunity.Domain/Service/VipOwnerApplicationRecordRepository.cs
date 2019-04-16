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

                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息不正确！");
                }
                var smallDistricts = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistricts == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                if (!Guid.TryParse(dto.OwnerCertificationId, out var ownerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == ownerCertificationId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主认证信息不存在！");
                }

                var ownerName = await db.OwnerCertificationRecords.Where(x => x.UserId == dto.UserId && x.SmallDistrictId == dto.SmallDistrictId && x.IsDeleted == false).Select(x => x.OwnerName).FirstOrDefaultAsync(token);

                var vipOwnerApplicationRecord = await db.VipOwnerApplicationRecords.Where(x => x.UserId == dto.UserId && (x.IsDeleted == false || x.IsInvalid == false)).FirstOrDefaultAsync(token);
                if (vipOwnerApplicationRecord != null)
                {
                    throw new NotImplementedException("您已提交过申请！");
                }

                var entity = db.VipOwnerApplicationRecords.Add(new VipOwnerApplicationRecord
                {
                    Reason = dto.Reason,
                    StructureId = dto.StructureId,
                    StructureName = vipOwnerStructure.Name,
                    UserId = dto.UserId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    SmallDistrictId = dto.SmallDistrictId,
                    SmallDistrictName = smallDistricts.Name,
                    LastOperationTime = dto.OperationTime,
                    Name = ownerName,
                    LastOperationUserId = dto.OperationUserId,
                    OwnerCertificationId = dto.OwnerCertificationId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task Adopt(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("申请记录Id信息不正确！");
                }
                var vipOwnerApplicationRecord = await db.VipOwnerApplicationRecords.Where(x => x.Id == uid && x.IsDeleted == false && x.IsAdopt == false).FirstOrDefaultAsync(token);
                if (vipOwnerApplicationRecord == null)
                {
                    throw new NotImplementedException("该申请记录不存在！");
                }

                vipOwnerApplicationRecord.IsAdopt = true;
                vipOwnerApplicationRecord.LastOperationTime = dto.OperationTime;
                vipOwnerApplicationRecord.LastOperationUserId = dto.OperationUserId;

                await db.SaveChangesAsync(token);
            }
        }

        public Task DeleteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VipOwnerApplicationRecord>> GetAllAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.IsInvalid == false && x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);
            }
        }

        public async Task<List<VipOwnerApplicationRecord>> GetAllForSmallDistrictIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.IsInvalid == false && x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);
            }
        }

        public async Task<VipOwnerApplicationRecord> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.VipOwnerApplicationRecords.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                return new VipOwnerApplicationRecord();
            }
        }

        public async Task<List<VipOwnerApplicationRecord>> GetListAdoptAsync(List<string> dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.IsInvalid == false && x.IsDeleted == false && x.IsAdopt == true && dto.Contains(x.UserId)).ToListAsync(token);
            }
        }

        public async Task<List<VipOwnerApplicationRecord>> GetListAsync(List<string> dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.IsInvalid == false && x.IsDeleted == false && dto.Contains(x.UserId)).ToListAsync(token);
            }
        }

        public async Task<List<VipOwnerApplicationRecord>> GetListAsync(string userId, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.IsInvalid == false && x.IsDeleted == false && x.UserId == userId).ToListAsync(token);
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

        public void OnSubscribe(SmallDistrictIncrementer incrementer)
        {
            incrementer.SmallDistrictEvent += SmallDistrictChanging;//在发布者私有委托里增加方法
        }

        public async void SmallDistrictChanging(GuoGuoCommunityContext dbs, SmallDistrict smallDistrict, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.VipOwnerApplicationRecords.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new VipOwnerApplicationRecord { SmallDistrictName = smallDistrict.Name });
            }
        }

        public async Task UpdateVoteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("申请记录Id信息不正确！");
                }
                var vipOwnerApplicationRecord = await db.VipOwnerApplicationRecords.Where(x => x.Id == uid && x.IsDeleted == false && x.IsAdopt == true).FirstOrDefaultAsync(token);
                if (vipOwnerApplicationRecord == null)
                {
                    throw new NotImplementedException("该申请记录不存在！");
                }

                vipOwnerApplicationRecord.VoteId = dto.VoteId;
                vipOwnerApplicationRecord.VoteQuestionId = dto.VoteQuestionId;
                vipOwnerApplicationRecord.VoteQuestionOptionId = dto.VoteQuestionOptionId;
                vipOwnerApplicationRecord.LastOperationTime = dto.OperationTime;
                vipOwnerApplicationRecord.LastOperationUserId = dto.OperationUserId;
                vipOwnerApplicationRecord.IsInvalid = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<VipOwnerApplicationRecord> GetForVoteQuestionIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.VoteId == dto.VoteId && x.VoteQuestionId == dto.VoteQuestionId).FirstOrDefaultAsync(token);
            }
        }

        public async Task<VipOwnerApplicationRecord> GetForVoteQuestionOptionIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerApplicationRecords.Where(x => x.VoteId == dto.VoteId && x.VoteQuestionId == dto.VoteQuestionId&&x.VoteQuestionOptionId==dto.VoteQuestionOptionId).FirstOrDefaultAsync(token);
            }
        }
    }
}
