using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 业户信息管理
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndustryController : ApiController
    {
        private readonly IIndustryRepository _industryRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="industryRepository"></param>
        public IndustryController(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加业户信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("industry/add")]
        public async Task<ApiResult<AddIndustryOutput>> Add([FromBody]AddIndustryInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddIndustryOutput>(APIResultCode.Unknown, new AddIndustryOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("业户门号信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Acreage))
                {
                    throw new NotImplementedException("业户面积信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Oriented))
                {
                    throw new NotImplementedException("业户朝向信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.NumberOfLayers))
                {
                    throw new NotImplementedException("业户层数信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingId))
                {
                    throw new NotImplementedException("业户楼宇Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingUnitId))
                {
                    throw new NotImplementedException("业户楼宇单元id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddIndustryOutput>(APIResultCode.Unknown, new AddIndustryOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _industryRepository.AddAsync(new IndustryDto
                {
                    Name = input.Name,
                    Oriented = input.Oriented,
                    NumberOfLayers = input.NumberOfLayers,
                    BuildingUnitId = input.BuildingUnitId,
                    BuildingId = input.BuildingId,
                    Acreage = input.Acreage,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddIndustryOutput>(APIResultCode.Success, new AddIndustryOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddIndustryOutput>(APIResultCode.Success_NoB, new AddIndustryOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 删除业户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("industry/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("业户Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _industryRepository.DeleteAsync(new IndustryDto
                {
                    Id = id,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        /// <summary>
        /// 修改业户信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("industry/update")]
        public async Task<ApiResult> Update([FromBody]UpdateIndustryInput input, CancellationToken cancellationToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }

                await _industryRepository.UpdateAsync(new IndustryDto
                {
                    Id = input.Id,
                    Name = input.Name,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                });

                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }

        }

        /// <summary>
        /// 获取业户详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("industry/get")]
        public async Task<ApiResult<GetIndustryOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                var data = await _industryRepository.GetAsync(id, cancelToken);

                return new ApiResult<GetIndustryOutput>(APIResultCode.Success, new GetIndustryOutput
                {
                    Id = data.Id.ToString(),
                    Name = data.Name,
                    Oriented = data.Oriented,
                    NumberOfLayers = data.NumberOfLayers,
                    BuildingId = data.BuildingId,
                    Acreage = data.Acreage,
                    BuildingUnitId = data.BuildingUnitId,
                    BuildingName = data.BuildingName,
                    BuildingUnitName = data.BuildingUnitName
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetIndustryOutput>(APIResultCode.Success_NoB, new GetIndustryOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询业户列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("industry/getAll")]
        public async Task<ApiResult<GetAllIndustryOutput>> GetAll([FromUri]GetAllIndustryInput input, CancellationToken cancelToken)
        {
            try
            {
                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var data = await _industryRepository.GetAllAsync(new IndustryDto
                {
                    Name = input?.Name,
                    BuildingId = input?.BuildingId,
                    BuildingUnitId = input?.BuildingUnitId
                }, cancelToken);

                return new ApiResult<GetAllIndustryOutput>(APIResultCode.Success, new GetAllIndustryOutput
                {
                    List = data.Select(x => new GetIndustryOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Acreage = x.Acreage,
                        BuildingId = x.BuildingId,
                        Oriented = x.Oriented,
                        BuildingName = x.BuildingName,
                        BuildingUnitId = x.BuildingUnitId,
                        BuildingUnitName = x.BuildingUnitName,
                        NumberOfLayers = x.NumberOfLayers
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllIndustryOutput>(APIResultCode.Success_NoB, new GetAllIndustryOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据楼宇单元id获取业户(小程序可用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("industry/getList")]
        public async Task<ApiResult<List<GetListIndustryOutput>>> GetList([FromUri]GetListIndustryInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.BuildingUnitId))
                {
                    throw new NotImplementedException("楼宇单元Id信息为空！");
                }

                var data = await _industryRepository.GetListAsync(new IndustryDto
                {
                    BuildingUnitId = input.BuildingUnitId
                }, cancelToken);

                return new ApiResult<List<GetListIndustryOutput>>(APIResultCode.Success, data.Select(x => new GetListIndustryOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListIndustryOutput>>(APIResultCode.Success_NoB, new List<GetListIndustryOutput> { }, e.Message);
            }
        }
    }
}
