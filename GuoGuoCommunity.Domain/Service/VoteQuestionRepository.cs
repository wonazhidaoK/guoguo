using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class VoteQuestionRepository : IVoteQuestionRepository
    {
        public async Task<VoteQuestion> AddAsync(VoteQuestionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.VoteQuestions.Add(new VoteQuestion
                {
                    OptionMode = dto.OptionMode,
                    VoteId = dto.VoteId,
                    Title = dto.Title,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteQuestionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteQuestion>> GetAllAsync(VoteQuestionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteQuestion> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteQuestion>> GetListAsync(VoteQuestionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteQuestionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
