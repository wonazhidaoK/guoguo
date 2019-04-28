using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Service;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ICityRepository
    {
        List<CityDto> GetState();

        List<CityDto> GetCity(CityDto dto);

        List<CityDto> GetRegion(RegionDto dto);

        List<ModelCountryState> Linkage(RegionDto dto);
    }
}
