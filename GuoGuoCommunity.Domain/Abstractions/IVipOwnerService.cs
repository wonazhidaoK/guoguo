using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerService
    {
        Task<VipOwner> AddAsync(VipOwnerDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<List<VipOwner>> GetAllAsync(VipOwnerDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<VipOwner> GetAsync(string id, CancellationToken token = default);

        Task<List<VipOwner>> GetListAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<List<VipOwner>> GetIsValidAsync(VipOwnerDto dto, CancellationToken token = default);
    }
}
