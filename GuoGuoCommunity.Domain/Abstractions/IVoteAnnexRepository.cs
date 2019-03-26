using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteAnnexRepository
    {
        Task<VoteAnnex> AddAsync(VoteAnnexDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteAnnexDto dto, CancellationToken token = default);

        Task<List<VoteAnnex>> GetAllAsync(VoteAnnexDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteAnnexDto dto, CancellationToken token = default);

        Task<VoteAnnex> GetAsync(string id, CancellationToken token = default);

        string GetUrl(string id);

        Task<List<VoteAnnex>> GetListAsync(VoteAnnexDto dto, CancellationToken token = default);
    }
}
