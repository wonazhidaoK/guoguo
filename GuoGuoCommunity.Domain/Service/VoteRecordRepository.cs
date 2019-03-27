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
    public class VoteRecordRepository : IVoteRecordRepository
    {
        public async Task<VoteRecord> AddAsync(VoteRecordDto dto, CancellationToken token = default)
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

                var entity = db.VoteRecords.Add(new VoteRecord
                {
                    Feedback = dto.Feedback,
                    VoteId = dto.VoteId,
                    OwnerCertificationId = dto.OwnerCertificationId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecord>> GetAllAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecord>> GetListAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
