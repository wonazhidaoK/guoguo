using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerService
    {
        Task<Owner> AddAsync(OwnerDto dto, CancellationToken token = default);

        Task UpdateAsync(OwnerDto dto, CancellationToken token = default);

        Task<List<Owner>> GetAllAsync(OwnerDto dto, CancellationToken token = default);

        Task DeleteAsync(OwnerDto dto, CancellationToken token = default);

        Task<Owner> GetAsync(string id, CancellationToken token = default);

        Task<List<Owner>> GetListAsync(OwnerDto dto, CancellationToken token = default);
    }
}
