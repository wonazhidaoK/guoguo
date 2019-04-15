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
    public class VoteResultRecordRepository : IVoteResultRecordRepository
    {
        public async Task<VoteResultRecord> AddAsync(VoteResultRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.VoteId, out var voteId))
                {
                    throw new NotImplementedException("投票Id信息不正确！");
                }
                var vote = await db.Votes.Where(x => x.Id == voteId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vote == null)
                {
                    throw new NotImplementedException("投票信息不存在！");
                }

                var duilding = await db.VoteResultRecords.Where(x => x.VoteId == dto.VoteId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (duilding != null)
                {
                    throw new NotImplementedException("该投票结果信息已存在！");
                }
                var entity = db.VoteResultRecords.Add(new VoteResultRecord
                {
                    CalculationMethodName = dto.CalculationMethodName,
                    CalculationMethodValue = dto.CalculationMethodValue,
                    ResultName = dto.ResultName,
                    ResultValue = dto.ResultValue,
                    VoteId = dto.VoteId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    ActualParticipateCount = dto.ActualParticipateCount,
                    ShouldParticipateCount = dto.ShouldParticipateCount,
                    VoteQuestionId = dto.VoteQuestionId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task<VoteResultRecord> AddVipOwnerElectionAsync(VoteResultRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.VoteId, out var voteId))
                {
                    throw new NotImplementedException("投票Id信息不正确！");
                }
                var vote = await db.Votes.Where(x => x.Id == voteId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vote == null)
                {
                    throw new NotImplementedException("投票信息不存在！");
                }

                var duilding = await db.VoteResultRecords.Where(x => x.VoteId == dto.VoteId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (duilding != null)
                {
                    throw new NotImplementedException("该投票结果信息已存在！");
                }
                var entity = db.VoteResultRecords.Add(new VoteResultRecord
                {
                    CalculationMethodName = dto.CalculationMethodName,
                    CalculationMethodValue = dto.CalculationMethodValue,
                    ResultName = dto.ResultName,
                    ResultValue = dto.ResultValue,
                    VoteId = dto.VoteId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    VoteQuestionId = dto.VoteQuestionId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteResultRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteResultRecord>> GetAllAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResultRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<VoteResultRecord> GetForVoteIdAsync(string voteId, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VoteResultRecords.Where(x => x.VoteId == voteId).FirstOrDefaultAsync(token);
            }
        }

        public async Task<List<VoteResultRecord>> GetListForVoteIdAsync(string voteId, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VoteResultRecords.Where(x => x.VoteId == voteId ).ToListAsync(token);
            }
        }

        public async Task<VoteResultRecord> GetForVoteQuestionIdAsync(VoteResultRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VoteResultRecords.Where(x => x.VoteId == dto.VoteId && x.VoteQuestionId == dto.VoteQuestionId).FirstOrDefaultAsync(token);
            }
        }

        public Task<List<VoteResultRecord>> GetListAsync(VoteResultRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteResultRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
