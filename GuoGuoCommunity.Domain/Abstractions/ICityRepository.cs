using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Service;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ICityRepository
    {
        /// <summary>
        /// 获取省份集合
        /// </summary>
        /// <returns></returns>
        List<CityDto> GetState();

        /// <summary>
        /// 获取城市集合 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        List<CityDto> GetCity(CityDto dto);

        /// <summary>
        /// 获取区集合
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        List<CityDto> GetRegion(RegionDto dto);

        /// <summary>
        /// 获取全部省市区数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        List<ModelCountryState> Linkage(RegionDto dto);
    }
}
