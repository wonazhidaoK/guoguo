using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IBuildingUnitRepository
    {
        Task<BuildingUnit> AddAsync(BuildingUnitDto dto, CancellationToken token = default);

        Task UpdateAsync(BuildingUnitDto dto, CancellationToken token = default);

        Task<List<BuildingUnit>> GetAllAsync(BuildingUnitDto dto, CancellationToken token = default);

        Task DeleteAsync(BuildingUnitDto dto, CancellationToken token = default);

        Task<BuildingUnit> GetAsync(string id, CancellationToken token = default);

        Task<List<BuildingUnit>> GetListAsync(BuildingUnitDto dto, CancellationToken token = default);
    }
}
