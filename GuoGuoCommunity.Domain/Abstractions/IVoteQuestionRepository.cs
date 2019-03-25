using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteQuestionRepository
    {
        Task<VoteQuestion> AddAsync(VoteQuestionDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteQuestionDto dto, CancellationToken token = default);

        Task<List<VoteQuestion>> GetAllAsync(VoteQuestionDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteQuestionDto dto, CancellationToken token = default);

        Task<VoteQuestion> GetAsync(string id, CancellationToken token = default);

        Task<List<VoteQuestion>> GetListAsync(VoteQuestionDto dto, CancellationToken token = default);
    }
}
