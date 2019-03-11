using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ICommunityService
    {
        Task<Community> AddAsync(CommunityDto dto, CancellationToken token = default);

        Task UpdateAsync(CommunityDto dto, CancellationToken token = default);

        Task<List<Community>> GetAllAsync(CommunityDto dto, CancellationToken token = default);

        Task DeleteAsync(CommunityDto dto, CancellationToken token = default);

        Task<Community> GetAsync(string id, CancellationToken token = default);

        Task<List<Community>> GetListAsync(CommunityDto dto, CancellationToken token = default);
    }
}
