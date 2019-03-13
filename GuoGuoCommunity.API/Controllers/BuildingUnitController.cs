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
    /// 楼宇单元信息管理
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BuildingUnitController : ApiController
    {
        private readonly IBuildingUnitService _buildingUnitService;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingUnitService"></param>
        public BuildingUnitController(IBuildingUnitService buildingUnitService)
        {
            _buildingUnitService = buildingUnitService;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加楼宇单元信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("buildingUnit/add")]
        public async Task<ApiResult<AddBuildingUnitOutput>> Add([FromBody]AddBuildingUnitInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    throw new NotImplementedException("token为空！");
                }
                if (string.IsNullOrWhiteSpace(input.UnitName))
                {
                    throw new NotImplementedException("楼宇单元信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.NumberOfLayers))
                {
                    throw new NotImplementedException("楼宇层信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingId))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }


                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    throw new NotImplementedException("token无效！");
                }

                var entity = await _buildingUnitService.AddAsync(new BuildingUnitDto
                {
                    BuildingId = input.BuildingId,
                    NumberOfLayers = input.NumberOfLayers,
                    UnitName = input.UnitName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddBuildingUnitOutput>(APIResultCode.Success, new AddBuildingUnitOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddBuildingUnitOutput>(APIResultCode.Success_NoB, new AddBuildingUnitOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 删除楼宇单元信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("buildingUnit/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    throw new NotImplementedException("token信息为空！");
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇单元Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    throw new NotImplementedException("token无效！");
                }
                await _buildingUnitService.DeleteAsync(new BuildingUnitDto
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
        /// 修改楼宇单元信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("buildingUnit/update")]
        public async Task<ApiResult> Update([FromBody]UpdateBuildingUnitInput input, CancellationToken cancellationToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    throw new NotImplementedException("token为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    throw new NotImplementedException("token无效！");
                }

                await _buildingUnitService.UpdateAsync(new BuildingUnitDto
                {
                    Id = input.Id,
                    NumberOfLayers = input.NumberOfLayers,
                    UnitName = input.UnitName,
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
        /// 获取楼宇单元详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("buildingUnit/get")]
        public async Task<ApiResult<GetBuildingUnitOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                var data = await _buildingUnitService.GetAsync(id, cancelToken);

                return new ApiResult<GetBuildingUnitOutput>(APIResultCode.Success, new GetBuildingUnitOutput
                {
                    Id = data.Id.ToString(),
                    NumberOfLayers = data.NumberOfLayers,
                    UnitName = data.UnitName
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetBuildingUnitOutput>(APIResultCode.Success_NoB, new GetBuildingUnitOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询楼宇单元列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("buildingUnit/getAll")]
        public async Task<ApiResult<GetAllBuildingUnitOutput>> GetAll([FromUri]GetAllBuildingUnitInput input, CancellationToken cancelToken)
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
                var data = await _buildingUnitService.GetAllAsync(new BuildingUnitDto
                {
                    BuildingId = input.BuildingId,
                    NumberOfLayers = input.NumberOfLayers,
                    UnitName = input.UnitName
                }, cancelToken);

                return new ApiResult<GetAllBuildingUnitOutput>(APIResultCode.Success, new GetAllBuildingUnitOutput
                {
                    List = data.Select(x => new GetBuildingUnitOutput
                    {
                        Id = x.Id.ToString(),
                        UnitName = x.UnitName,
                        NumberOfLayers = x.NumberOfLayers
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllBuildingUnitOutput>(APIResultCode.Success_NoB, new GetAllBuildingUnitOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据楼宇id获取楼宇单元列表(小程序可用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("buildingUnit/getList")]
        public async Task<ApiResult<List<GetListBuildingUnitOutput>>> GetList([FromUri]GetListBuildingUnitInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.BuildingId))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }

                var data = await _buildingUnitService.GetListAsync(new BuildingUnitDto
                {
                    BuildingId = input.BuildingId
                }, cancelToken);

                return new ApiResult<List<GetListBuildingUnitOutput>>(APIResultCode.Success, data.Select(x => new GetListBuildingUnitOutput
                {
                    Id = x.Id.ToString(),
                    UnitName = x.UnitName
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListBuildingUnitOutput>>(APIResultCode.Success_NoB, new List<GetListBuildingUnitOutput> { }, e.Message);
            }
        }
    }
}
