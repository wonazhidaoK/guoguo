using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 城市
    /// </summary>
    public class CityController : BaseController
    {
        private readonly ICityRepository _cityService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityService"></param>
        public CityController(ICityRepository cityService)
        {
            _cityService = cityService;
        }

        /// <summary>
        /// 获取城市下区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/getRegion")]
        public ApiResult<List<CityOutput>> GetRegion([FromUri]string stateName, [FromUri]string cityName)
        {
            return new ApiResult<List<CityOutput>>(APIResultCode.Success, _cityService.GetRegion(new RegionDto
            {
                CityDto = new CityDto
                {
                    Name = stateName
                },
                Name = cityName
            }).Select(
                x => new CityOutput
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList());
        }

        /// <summary>
        /// 获取省份下城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/getCity")]
        public ApiResult<List<CityOutput>> GetCity([FromUri]string stateName)
        {
            return new ApiResult<List<CityOutput>>(APIResultCode.Success, _cityService.GetCity(new CityDto
            {
                Name = stateName
            }).Select(
               x => new CityOutput
               {
                   Code = x.Code,
                   Name = x.Name
               }).ToList());

        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/getState")]
        public ApiResult<List<CityOutput>> GetState()
        {
            return new ApiResult<List<CityOutput>>(APIResultCode.Success, _cityService.GetState().Select(
              x => new CityOutput
              {
                  Code = x.Code,
                  Name = x.Name
              }).ToList());

        }
        /// <summary>
        /// 获取全部城市数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/linkage")]
        public List<ModelCountryState> Linkage()
        {
            var city = _cityService.Linkage(new RegionDto());
            return city;
        }
    }
}
