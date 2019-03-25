using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class VoteQuestionOptionRepository : IVoteQuestionOptionRepository
    {
        public async Task<VoteQuestionOption> AddAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.VoteQuestionOptions.Add(new VoteQuestionOption
                {

                    VoteId = dto.VoteId,
                    Describe = dto.Describe,
                    VoteQuestionId = dto.VoteQuestionId,
                    Votes = 0,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteQuestionOption>> GetAllAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteQuestionOption> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteQuestionOption>> GetListAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
