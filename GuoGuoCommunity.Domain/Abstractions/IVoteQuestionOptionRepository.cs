using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteQuestionOptionRepository
    {
        Task<VoteQuestionOption> AddAsync(VoteQuestionOptionDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteQuestionOptionDto dto, CancellationToken token = default);

        Task<List<VoteQuestionOption>> GetAllAsync(VoteQuestionOptionDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteQuestionOptionDto dto, CancellationToken token = default);

        Task<VoteQuestionOption> GetAsync(string id, CancellationToken token = default);

        Task<List<VoteQuestionOption>> GetListAsync(VoteQuestionOptionDto dto, CancellationToken token = default);
    }
}
