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
    /// 业委会架构(职能)
    /// </summary>
    public class VipOwnerStructureController : BaseController
    {
        private readonly IVipOwnerStructureRepository _vipOwnerStructureRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerStructureRepository"></param>
        public VipOwnerStructureController(IVipOwnerStructureRepository vipOwnerStructureRepository)
        {
            _vipOwnerStructureRepository = vipOwnerStructureRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加职能信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwnerStructure/add")]
        public async Task<ApiResult<AddVipOwnerStructureOutput>> Add([FromBody]AddVipOwnerStructureInput input, CancellationToken cancelToken)
        {

            if (Authorization == null)
            {
                return new ApiResult<AddVipOwnerStructureOutput>(APIResultCode.Unknown, new AddVipOwnerStructureOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("职能名称信息为空！");
            }

            if (string.IsNullOrWhiteSpace(input.Weights))
            {
                throw new NotImplementedException("职能权重信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddVipOwnerStructureOutput>(APIResultCode.Unknown, new AddVipOwnerStructureOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _vipOwnerStructureRepository.AddAsync(new VipOwnerStructureDto
            {
                Name = input.Name,
                Description = input.Description,
                IsReview = input.IsReview.Value,
                Weights = input.Weights,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddVipOwnerStructureOutput>(APIResultCode.Success, new AddVipOwnerStructureOutput { Id = entity.Id.ToString() });

        }

        /// <summary>
        /// 修改职能信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwnerStructure/update")]
        public async Task<ApiResult> Update([FromBody]UpdateVipOwnerStructureInput input, CancellationToken cancellationToken)
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

            await _vipOwnerStructureRepository.UpdateAsync(new VipOwnerStructureDto
            {
                Id = input.Id,
                Name = input.Name,
                Description = input.Description,
                IsReview = input.IsReview,
                Weights = input.Weights,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });

            return new ApiResult();
        }

        /// <summary>
        /// 删除职能信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerStructure/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("社区Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _vipOwnerStructureRepository.DeleteAsync(new VipOwnerStructureDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 获取职能详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerStructure/get")]
        public async Task<ApiResult<GetVipOwnerStructureOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetVipOwnerStructureOutput>(APIResultCode.Unknown, new GetVipOwnerStructureOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("社区Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetVipOwnerStructureOutput>(APIResultCode.Unknown, new GetVipOwnerStructureOutput { }, APIResultMessage.TokenError);
            }
            var data = await _vipOwnerStructureRepository.GetAsync(id, cancelToken);

            return new ApiResult<GetVipOwnerStructureOutput>(APIResultCode.Success, new GetVipOwnerStructureOutput
            {
                Id = data.Id.ToString(),
                Description = data.Description,
                Name = data.Name,
                IsReview = data.IsReview,
                Weights = data.Weights
            });
        }

        /// <summary>
        /// 查询职能列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerStructure/getAll")]
        public async Task<ApiResult<GetAllVipOwnerStructureOutput>> GetAll([FromUri]GetAllVipOwnerStructureInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllVipOwnerStructureOutput>(APIResultCode.Unknown, new GetAllVipOwnerStructureOutput { }, APIResultMessage.TokenNull);
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

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllVipOwnerStructureOutput>(APIResultCode.Unknown, new GetAllVipOwnerStructureOutput { }, APIResultMessage.TokenError);
            }
            var data = await _vipOwnerStructureRepository.GetAllAsync(new VipOwnerStructureDto
            {
                Name = input?.Name,
                Weights = input.Weights,
                IsReview = input.IsReview,
                Description = input.Description
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllVipOwnerStructureOutput>(APIResultCode.Success, new GetAllVipOwnerStructureOutput
            {
                List = list.Select(x => new GetVipOwnerStructureOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    IsReview = x.IsReview,
                    Weights = x.Weights
                }).ToList(),
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 获取职能(小程序可用)
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerStructure/getList")]
        public async Task<ApiResult<List<GetListVipOwnerStructureOutput>>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListVipOwnerStructureOutput>>(APIResultCode.Unknown, new List<GetListVipOwnerStructureOutput> { }, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListVipOwnerStructureOutput>>(APIResultCode.Unknown, new List<GetListVipOwnerStructureOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _vipOwnerStructureRepository.GetListAsync(cancelToken);
            return new ApiResult<List<GetListVipOwnerStructureOutput>>(APIResultCode.Success, data.Select(x => new GetListVipOwnerStructureOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}
