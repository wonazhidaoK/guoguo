using GuoGuoCommunity.API.Models;
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
    /// 楼宇信息管理
    /// </summary>
    public class BuildingController : BaseController
    {
        private readonly IBuildingRepository _buildingService;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public BuildingController(IBuildingRepository buildingService, ITokenRepository tokenRepository)
        {
            _buildingService = buildingService;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// 添加楼宇信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("building/add")]
        public async Task<ApiResult<AddBuildingOutput>> Add([FromBody]AddBuildingInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddBuildingOutput>(APIResultCode.Unknown, new AddBuildingOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("楼宇名称信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
            {
                throw new NotImplementedException("楼宇小区Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddBuildingOutput>(APIResultCode.Unknown, new AddBuildingOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _buildingService.AddAsync(new BuildingDto
            {
                Name = input.Name,
                SmallDistrictId = input.SmallDistrictId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddBuildingOutput>(APIResultCode.Success, new AddBuildingOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 删除楼宇信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("building/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _buildingService.DeleteAsync(new BuildingDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 修改楼宇信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("building/update")]
        public async Task<ApiResult> Update([FromBody]UpdateBuildingInput input, CancellationToken cancellationToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("楼宇名称信息为空！");
            }
            await _buildingService.UpdateAsync(new BuildingDto
            {
                Id = input.Id,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancellationToken);

            return new ApiResult();
        }

        /// <summary>
        /// 获取楼宇详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("building/get")]
        public async Task<ApiResult<GetBuildingOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetBuildingOutput>(APIResultCode.Unknown, new GetBuildingOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetBuildingOutput>(APIResultCode.Unknown, new GetBuildingOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }
            var data = await _buildingService.GetIncludeAsync(id, cancelToken);

            return new ApiResult<GetBuildingOutput>(APIResultCode.Success, new GetBuildingOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                SmallDistrictId = data.SmallDistrictId.ToString(),
                SmallDistrictName = data.SmallDistrict.Name
            });
        }

        /// <summary>
        /// 查询楼宇列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("building/getAll")]
        public async Task<ApiResult<GetAllBuildingOutput>> GetAll([FromUri]GetAllBuildingInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllBuildingOutput>(APIResultCode.Unknown, new GetAllBuildingOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllBuildingOutput>(APIResultCode.Unknown, new GetAllBuildingOutput { }, APIResultMessage.TokenError);
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

            var data = await _buildingService.GetAllIncludeAsync(new BuildingDto
            {
                Name = input?.Name,
                SmallDistrictId = input.SmallDistrictId
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllBuildingOutput>(APIResultCode.Success, new GetAllBuildingOutput
            {
                List = list.Select(x => new GetBuildingOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    SmallDistrictId = x.SmallDistrictId.ToString(),
                    SmallDistrictName = x.SmallDistrict.Name,
                }).ToList(),
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 根据小区获取楼宇
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("building/getList")]
        public async Task<ApiResult<List<GetListBuildingOutput>>> GetList([FromUri]GetListBuildingInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListBuildingOutput>>(APIResultCode.Unknown, new List<GetListBuildingOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListBuildingOutput>>(APIResultCode.Unknown, new List<GetListBuildingOutput> { }, APIResultMessage.TokenError);
            }

            if (string.IsNullOrWhiteSpace(input?.SmallDistrictId))
            {
                throw new NotImplementedException("小区信息为空！");
            }

            var data = await _buildingService.GetListAsync(new BuildingDto
            {
                SmallDistrictId = input.SmallDistrictId
            }, cancelToken);

            return new ApiResult<List<GetListBuildingOutput>>(APIResultCode.Success, data.Select(x => new GetListBuildingOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}
