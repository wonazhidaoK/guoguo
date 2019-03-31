using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteAssociationVipOwnerRepository
    {
        Task<VoteAssociationVipOwner> AddAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default);

        Task<List<VoteAssociationVipOwner>> GetAllAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default);

        Task<VoteAssociationVipOwner> GetAsync(string id, CancellationToken token = default);

        Task<VoteAssociationVipOwner> GetForVoteIdAsync(string id, CancellationToken token = default);

        Task<List<VoteAssociationVipOwner>> GetListAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default);
    }
}
