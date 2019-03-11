using GuoGuoCommunity.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ICityService
    {
        Task<List<CityDto>> GetState();

        Task<List<CityDto>> GetCity(CityDto dto);

        Task<List<CityDto>> GetRegion(RegionDto dto);
    }
}
