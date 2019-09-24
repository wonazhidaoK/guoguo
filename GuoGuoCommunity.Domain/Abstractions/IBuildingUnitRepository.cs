using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IBuildingUnitRepository : IIncludeRepository<BuildingUnit, BuildingUnitDto>
    {
        /// <summary>
        /// 更改楼宇单元层数信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<BuildingUnit> UpdateNumberOfLayersAsync(BuildingUnitDto dto, CancellationToken token = default);
    }
}
