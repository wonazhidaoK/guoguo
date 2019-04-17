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
    /// 楼宇信息管理
    /// </summary>
    public class BuildingController : ApiController
    {
        private readonly IBuildingRepository _buildingService;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        public BuildingController(IBuildingRepository buildingService)
        {
            _buildingService = buildingService;
            _tokenManager = new TokenManager();
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
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
                if (string.IsNullOrWhiteSpace(input.SmallDistrictName))
                {
                    throw new NotImplementedException("楼宇小区名称信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddBuildingOutput>(APIResultCode.Unknown, new AddBuildingOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _buildingService.AddAsync(new BuildingDto
                {
                    Name = input.Name,
                    SmallDistrictId = input.SmallDistrictId,
                    SmallDistrictName = input.SmallDistrictName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddBuildingOutput>(APIResultCode.Success, new AddBuildingOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddBuildingOutput>(APIResultCode.Success_NoB, new AddBuildingOutput { }, e.Message);
            }
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
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
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
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

                await _buildingService.UpdateAsync(new BuildingDto
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
        /// 获取楼宇详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("building/get")]
        public async Task<ApiResult<GetBuildingOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                var data = await _buildingService.GetAsync(id, cancelToken);

                return new ApiResult<GetBuildingOutput>(APIResultCode.Success, new GetBuildingOutput
                {
                    Id = data.Id.ToString(),
                    Name = data.Name,
                    SmallDistrictId = data.SmallDistrictId,
                    SmallDistrictName = data.SmallDistrictName
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetBuildingOutput>(APIResultCode.Success_NoB, new GetBuildingOutput { }, e.Message);
            }
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
                if (!string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("楼宇小区Id信息为空！");
                }
                var data = await _buildingService.GetAllAsync(new BuildingDto
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
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                    }).ToList(),
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllBuildingOutput>(APIResultCode.Success_NoB, new GetAllBuildingOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据小区获取楼宇(小程序可用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("building/getList")]
        public async Task<ApiResult<List<GetListBuildingOutput>>> GetList([FromUri]GetListBuildingInput input, CancellationToken cancelToken)
        {
            try
            {
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
            catch (Exception e)
            {
                return new ApiResult<List<GetListBuildingOutput>>(APIResultCode.Success_NoB, new List<GetListBuildingOutput> { }, e.Message);
            }
        }
    }
}
