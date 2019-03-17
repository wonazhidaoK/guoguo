using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 城市
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CityController : ApiController
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
        public async Task<ApiResult<List<CityOutput>>> GetRegion([FromUri]string stateName, [FromUri]string cityName)
        {
            try
            {
                return new ApiResult<List<CityOutput>>(APIResultCode.Success, (await _cityService.GetRegion(new RegionDto
                {
                    CityDto = new CityDto
                    {
                        Name = stateName,
                        // Code = stateCode
                    },
                    //Code = cityCode,
                    Name = cityName
                })).Select(
                    x => new CityOutput
                    {
                        Code = x.Code,
                        Name = x.Name
                    }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<CityOutput>>(APIResultCode.Success_NoB, new List<CityOutput> { }, e.Message);
            }

        }

        /// <summary>
        /// 获取省份下城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/getCity")]
        public async Task<ApiResult<List<CityOutput>>> GetCity([FromUri]string stateName)
        {
            try
            {
                return new ApiResult<List<CityOutput>>(APIResultCode.Success, (await _cityService.GetCity(new CityDto
                {
                    //Code = stateCode,
                    Name = stateName
                })).Select(
                   x => new CityOutput
                   {
                       Code = x.Code,
                       Name = x.Name
                   }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<CityOutput>>(APIResultCode.Success_NoB, new List<CityOutput> { }, e.Message);
            }
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("city/getState")]
        public async Task<ApiResult<List<CityOutput>>> GetState()
        {
            try
            {
                return new ApiResult<List<CityOutput>>(APIResultCode.Success, (await _cityService.GetState()).Select(
                  x => new CityOutput
                  {
                      Code = x.Code,
                      Name = x.Name
                  }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<CityOutput>>(APIResultCode.Success_NoB, new List<CityOutput> { }, e.Message);
            }
        }
    }
}
