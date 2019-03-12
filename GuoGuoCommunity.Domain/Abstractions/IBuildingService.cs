using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IBuildingService
    {
        Task<Building> AddAsync(BuildingDto dto, CancellationToken token = default);

        Task UpdateAsync(BuildingDto dto, CancellationToken token = default);

        Task<List<Building>> GetAllAsync(BuildingDto dto, CancellationToken token = default);

        Task DeleteAsync(BuildingDto dto, CancellationToken token = default);

        Task<Building> GetAsync(string id, CancellationToken token = default);

        Task<List<Building>> GetListAsync(BuildingDto dto, CancellationToken token = default);
    }
}
