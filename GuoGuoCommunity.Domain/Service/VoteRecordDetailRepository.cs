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
    public class VoteRecordDetailRepository : IVoteRecordDetailRepository
    {
        public async Task<VoteRecordDetail> AddAsync(VoteRecordDetailDto dto, CancellationToken token = default)
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

                if (!Guid.TryParse(dto.VoteQuestionId, out var voteQuestionId))
                {
                    throw new NotImplementedException("投票问题Id信息不正确！");
                }
                var voteQuestion = await db.VoteQuestions.Where(x => x.Id == voteQuestionId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (voteQuestion == null)
                {
                    throw new NotImplementedException("投票问题信息不存在！");
                }

                if (!Guid.TryParse(dto.VoteQuestionOptionId, out var voteQuestionOptionId))
                {
                    throw new NotImplementedException("投票问题选项Id信息不正确！");
                }
                var voteRecordDetail = await db.VoteRecordDetails.Where(x => x.Id == voteQuestionOptionId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (voteRecordDetail == null)
                {
                    throw new NotImplementedException("投票问题选项信息不存在！");
                }

                var entity = db.VoteRecordDetails.Add(new VoteRecordDetail
                {
                    VoteQuestionId = dto.VoteQuestionId,
                    VoteQuestionOptionId = dto.VoteQuestionOptionId,
                    VoteId = dto.VoteId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecordDetail>> GetAllAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteRecordDetail> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecordDetail>> GetListAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
