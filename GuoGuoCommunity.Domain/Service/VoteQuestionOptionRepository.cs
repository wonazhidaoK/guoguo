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

        public async Task<VoteQuestionOption> AddCountAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var uid))
                {
                    throw new NotImplementedException("选项Id信息不正确！");
                }
                var voteQuestionOption = await db.VoteQuestionOptions.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (voteQuestionOption == null)
                {
                    throw new NotImplementedException("该选项不存在！");
                }

                voteQuestionOption.Votes = voteQuestionOption.Votes + 1;
                
                await db.SaveChangesAsync(token);
                return voteQuestionOption;
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

        public async Task<VoteQuestionOption> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.VoteQuestionOptions.Where(x => x.IsDeleted == false && x.Id == uid ).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该投票问题选项Id信息不正确！");
            }
        }

        public async Task<List<VoteQuestionOption>> GetListAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VoteQuestionOptions.Where(x => x.IsDeleted == false && x.VoteId == dto.VoteId && x.VoteQuestionId == dto.VoteQuestionId).ToListAsync(token);
            }
        }

        public Task UpdateAsync(VoteQuestionOptionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
