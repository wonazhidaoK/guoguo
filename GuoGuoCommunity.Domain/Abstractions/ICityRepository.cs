using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ICityRepository
    {
        Task<List<CityDto>> GetState();

        Task<List<CityDto>> GetCity(CityDto dto);

        Task<List<CityDto>> GetRegion(RegionDto dto);

        Task<List<ModelCountryState>> Linkage(RegionDto dto);
    }
}
