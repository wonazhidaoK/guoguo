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

        Task<Vote> AddForVipOwnerAsync(VoteDto dto, CancellationToken token = default);

        Task<Vote> AddForStreetOfficeAsync(VoteDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteDto dto, CancellationToken token = default);

        Task UpdateForClosedAsync(VoteDto dto, CancellationToken token = default);

        Task UpdateCalculationMethodAsync(VoteDto dto, CancellationToken token = default);

        Task<List<Vote>> GetAllForStreetOfficeAsync(VoteDto dto, CancellationToken token = default);

        Task<List<Vote>> GetAllForPropertyAsync(VoteDto dto, CancellationToken token = default);

        Task<List<Vote>> GetAllForVipOwnerAsync(VoteDto dto, CancellationToken token = default);

        Task<List<Vote>> GetAllForOwnerAsync(VoteDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteDto dto, CancellationToken token = default);

        Task<Vote> GetAsync(string id, CancellationToken token = default);

        Task<List<Vote>> GetListAsync(VoteDto dto, CancellationToken token = default);

        Task<List<Vote>> GetDeadListAsync(VoteDto dto, CancellationToken token = default);
    }
}
