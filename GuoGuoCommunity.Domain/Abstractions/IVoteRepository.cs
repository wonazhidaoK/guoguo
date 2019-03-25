using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteRepository
    {
        Task<Vote> AddAsync(VoteDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteDto dto, CancellationToken token = default);

        Task<List<Vote>> GetAllAsync(VoteDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteDto dto, CancellationToken token = default);

        Task<Vote> GetAsync(string id, CancellationToken token = default);

        Task<List<Vote>> GetListAsync(VoteDto dto, CancellationToken token = default);
    }
}
