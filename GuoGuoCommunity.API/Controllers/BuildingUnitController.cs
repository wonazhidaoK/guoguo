using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 楼宇单元信息管理
    /// </summary>
    public class BuildingUnitController : BaseController
    {
        private readonly IBuildingUnitRepository _buildingUnitRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingUnitRepository"></param>
        public BuildingUnitController(IBuildingUnitRepository buildingUnitRepository)
        {
            _buildingUnitRepository = buildingUnitRepository;
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
            if (Authorization == null)
            {
                return new ApiResult<AddBuildingUnitOutput>(APIResultCode.Unknown, new AddBuildingUnitOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.UnitName))
            {
                throw new NotImplementedException("楼宇单元信息为空！");
            }
            if (input.NumberOfLayers == 0)
            {
                throw new NotImplementedException("楼宇层信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.BuildingId))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddBuildingUnitOutput>(APIResultCode.Unknown, new AddBuildingUnitOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _buildingUnitRepository.AddAsync(new BuildingUnitDto
            {
                BuildingId = input.BuildingId,
                NumberOfLayers = input.NumberOfLayers,
                UnitName = input.UnitName,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddBuildingUnitOutput>(APIResultCode.Success, new AddBuildingUnitOutput { Id = entity.Id.ToString() });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇单元Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _buildingUnitRepository.DeleteAsync(new BuildingUnitDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult();
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _buildingUnitRepository.UpdateAsync(new BuildingUnitDto
            {
                Id = input.Id,
                NumberOfLayers = input.NumberOfLayers,
                UnitName = input.UnitName,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });
            return new ApiResult();
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
            if (Authorization == null)
            {
                return new ApiResult<GetBuildingUnitOutput>(APIResultCode.Unknown, new GetBuildingUnitOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetBuildingUnitOutput>(APIResultCode.Unknown, new GetBuildingUnitOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }
            var data = await _buildingUnitRepository.GetAsync(id, cancelToken);

            return new ApiResult<GetBuildingUnitOutput>(APIResultCode.Success, new GetBuildingUnitOutput
            {
                Id = data.Id.ToString(),
                NumberOfLayers = data.NumberOfLayers,
                UnitName = data.UnitName
            });
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllBuildingUnitOutput>(APIResultCode.Unknown, new GetAllBuildingUnitOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllBuildingUnitOutput>(APIResultCode.Unknown, new GetAllBuildingUnitOutput { }, APIResultMessage.TokenError);
            }
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }
            int startRow = (input.PageIndex - 1) * input.PageSize;
            if (string.IsNullOrWhiteSpace(input.BuildingId))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }
            var data = await _buildingUnitRepository.GetAllAsync(new BuildingUnitDto
            {
                BuildingId = input.BuildingId,
                NumberOfLayers = input.NumberOfLayers,
                UnitName = input.UnitName
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllBuildingUnitOutput>(APIResultCode.Success, new GetAllBuildingUnitOutput
            {
                List = list.Select(x => new GetBuildingUnitOutput
                {
                    Id = x.Id.ToString(),
                    UnitName = x.UnitName,
                    NumberOfLayers = x.NumberOfLayers
                }).ToList(),
                TotalCount = list.Count()
            });

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
            if (Authorization == null)
            {
                return new ApiResult<List<GetListBuildingUnitOutput>>(APIResultCode.Unknown, new List<GetListBuildingUnitOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListBuildingUnitOutput>>(APIResultCode.Unknown, new List<GetListBuildingUnitOutput> { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(input.BuildingId))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }

            var data = await _buildingUnitRepository.GetListAsync(new BuildingUnitDto
            {
                BuildingId = input.BuildingId
            }, cancelToken);

            return new ApiResult<List<GetListBuildingUnitOutput>>(APIResultCode.Success, data.Select(x => new GetListBuildingUnitOutput
            {
                Id = x.Id.ToString(),
                UnitName = x.UnitName
            }).ToList());
        }
    }
}
