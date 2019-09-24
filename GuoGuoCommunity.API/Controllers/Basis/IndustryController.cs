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
    /// 业户信息管理
    /// </summary>
    public class IndustryController : BaseController
    {
        private readonly IIndustryRepository _industryRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public IndustryController(IIndustryRepository industryRepository, ITokenRepository tokenRepository)
        {
            _industryRepository = industryRepository;
            _tokenRepository = tokenRepository;
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
            if (Authorization == null)
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
            if (input.NumberOfLayers == 0)
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

            var user = _tokenRepository.GetUser(Authorization);
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

            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("业户Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
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
            }, cancellationToken);

            return new ApiResult();
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

            if (Authorization == null)
            {
                return new ApiResult<GetIndustryOutput>(APIResultCode.Unknown, new GetIndustryOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("业户Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetIndustryOutput>(APIResultCode.Unknown, new GetIndustryOutput { }, APIResultMessage.TokenError);
            }
            var data = await _industryRepository.GetIncludeAsync(id, cancelToken);

            return new ApiResult<GetIndustryOutput>(APIResultCode.Success, new GetIndustryOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                Oriented = data.Oriented,
                NumberOfLayers = data.NumberOfLayers,
                BuildingId = data.BuildingUnit.BuildingId.ToString(),
                Acreage = data.Acreage,
                BuildingUnitId = data.BuildingUnitId.ToString(),
                BuildingName = data.BuildingUnit.Building.Name,
                BuildingUnitName = data.BuildingUnit.UnitName
            });

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
            if (Authorization == null)
            {
                return new ApiResult<GetAllIndustryOutput>(APIResultCode.Unknown, new GetAllIndustryOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllIndustryOutput>(APIResultCode.Unknown, new GetAllIndustryOutput { }, APIResultMessage.TokenError);
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
            var data = await _industryRepository.GetAllIncludeAsync(new IndustryDto
            {
                Name = input?.Name,
                BuildingId = input?.BuildingId,
                BuildingUnitId = input?.BuildingUnitId,
                OperationUserSmallDistrictId = user.SmallDistrictId
            }, cancelToken);

            var listCount = data.Count();
            var list = data.OrderByDescending(x => x.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllIndustryOutput>(APIResultCode.Success, new GetAllIndustryOutput
            {
                List = list.Select(x => new GetIndustryOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Acreage = x.Acreage,
                    BuildingId = x.BuildingUnit.BuildingId.ToString(),
                    Oriented = x.Oriented,
                    BuildingName = x.BuildingUnit.Building.Name,
                    BuildingUnitId = x.BuildingUnitId.ToString(),
                    BuildingUnitName = x.BuildingUnit.UnitName,
                    NumberOfLayers = x.NumberOfLayers
                }).ToList(),
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 根据楼宇单元id层数获取业户(小程序可用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("industry/getList")]
        public async Task<ApiResult<List<GetListIndustryOutput>>> GetList([FromUri]GetListIndustryInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListIndustryOutput>>(APIResultCode.Unknown, new List<GetListIndustryOutput> { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.BuildingUnitId))
            {
                throw new NotImplementedException("楼宇单元Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListIndustryOutput>>(APIResultCode.Unknown, new List<GetListIndustryOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _industryRepository.GetListAsync(new IndustryDto
            {
                BuildingUnitId = input.BuildingUnitId,
                NumberOfLayers = input.NumberOfLayers
            }, cancelToken);

            return new ApiResult<List<GetListIndustryOutput>>(APIResultCode.Success, data.Select(x => new GetListIndustryOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}
