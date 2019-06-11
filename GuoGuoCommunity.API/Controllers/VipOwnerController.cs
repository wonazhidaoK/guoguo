using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 业委会管理
    /// </summary>
    public class VipOwnerController : BaseController
    {
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerRepository"></param>
        public VipOwnerController(IVipOwnerRepository vipOwnerRepository)
        {
            _vipOwnerRepository = vipOwnerRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加业委会信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwner/add")]
        public async Task<ApiResult<AddVipOwnerOutput>> Add([FromBody]AddVipOwnerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddVipOwnerOutput>(APIResultCode.Unknown, new AddVipOwnerOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
            {
                throw new NotImplementedException("业委会小区Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddVipOwnerOutput>(APIResultCode.Unknown, new AddVipOwnerOutput { }, APIResultMessage.TokenError);
            }

            if ((await _vipOwnerRepository.GetListAsync(new VipOwnerDto
            {
                SmallDistrictId = input.SmallDistrictId
            }, cancelToken)).Any())
            {
                throw new NotImplementedException("当前小区存在未竞选业委会！不能重复添加！");
            }

            var count = (await _vipOwnerRepository.GetIsValidAsync(new VipOwnerDto
            {
                SmallDistrictId = input.SmallDistrictId
            })).Count;

            var entity = await _vipOwnerRepository.AddAsync(new VipOwnerDto
            {
                Name = "【第" + (count + 1) + "届】业主委员会",
                RemarkName = input.RemarkName,
                SmallDistrictId = input.SmallDistrictId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult<AddVipOwnerOutput>(APIResultCode.Success, new AddVipOwnerOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 删除业委会信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("业委会Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _vipOwnerRepository.DeleteAsync(new VipOwnerDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 将业委会置为无效业委会信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/invalid")]
        public async Task<ApiResult> Invalid([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("业委会Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);

            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _vipOwnerRepository.UpdateInvalidAsync(new VipOwnerDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 修改业委会信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwner/update")]
        public async Task<ApiResult> Update([FromBody]UpdateVipOwnerInput input, CancellationToken cancellationToken)
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

            await _vipOwnerRepository.UpdateAsync(new VipOwnerDto
            {
                Id = input.Id,
                RemarkName = input.RemarkName,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });
            return new ApiResult();
        }

        /// <summary>
        /// 获取业委会详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/get")]
        public async Task<ApiResult<GetVipOwnerOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetVipOwnerOutput>(APIResultCode.Unknown, new GetVipOwnerOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetVipOwnerOutput>(APIResultCode.Unknown, new GetVipOwnerOutput { }, APIResultMessage.TokenError);
            }

            var data = await _vipOwnerRepository.GetAsync(id, cancelToken);

            return new ApiResult<GetVipOwnerOutput>(APIResultCode.Success, new GetVipOwnerOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                IsValid = data.IsValid,
                RemarkName = data.RemarkName,
                SmallDistrictId = data.SmallDistrictId.ToString(),
                SmallDistrictName = data.SmallDistrict.Name
            });
        }

        /// <summary>
        /// 查询业委会列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/getAll")]
        public async Task<ApiResult<GetAllVipOwnerOutput>> GetAll([FromUri]GetAllVipOwnerInput input, CancellationToken cancelToken)
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllVipOwnerOutput>(APIResultCode.Unknown, new GetAllVipOwnerOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllVipOwnerOutput>(APIResultCode.Unknown, new GetAllVipOwnerOutput { }, APIResultMessage.TokenError);
            }

            var data = await _vipOwnerRepository.GetAllAsync(new VipOwnerDto
            {
                Name = input.Name,
                SmallDistrictId = input.SmallDistrictId
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllVipOwnerOutput>(APIResultCode.Success, new GetAllVipOwnerOutput
            {
                List = list.Select(x => new GetVipOwnerOutput
                {
                    Id = x.Id.ToString(),
                    IsValid = x.IsValid,
                    RemarkName = x.RemarkName,
                    SmallDistrictId = x.SmallDistrictId.ToString(),
                    SmallDistrictName = x.SmallDistrict.Name,
                    Name = x.Name,
                    IsElection = x.IsElection,
                    IsCanDeleted = !x.IsElection,
                    IsCanInvalid = x.IsValid
                }).ToList(),
                TotalCount = listCount
            });

        }

        /// <summary>
        /// 根据小区获取业委会列表(高级认证用,获取当前小区未竞选的业委会)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/getList")]
        public async Task<ApiResult<List<GetListVipOwnerOutput>>> GetList([FromUri]GetListVipOwnerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Unknown, new List<GetListVipOwnerOutput> { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
            {
                throw new NotImplementedException("小区Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Unknown, new List<GetListVipOwnerOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _vipOwnerRepository.GetListAsync(new VipOwnerDto
            {
                SmallDistrictId = input.SmallDistrictId
            }, cancelToken);

            return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Success, data.Select(x => new GetListVipOwnerOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }

        /// <summary>
        /// 物业获取业委会列表
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/getListForProperty")]
        public async Task<ApiResult<List<GetListVipOwnerOutput>>> GetListForProperty(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Unknown, new List<GetListVipOwnerOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);

            if (user == null)
            {
                return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Unknown, new List<GetListVipOwnerOutput> { }, APIResultMessage.TokenError);
            }
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Success_NoB, new List<GetListVipOwnerOutput> { }, "操作人没有权限操作！");
            }
            var data = await _vipOwnerRepository.GetListForPropertyAsync(new VipOwnerDto
            {
                SmallDistrictId = user.SmallDistrictId
            }, cancelToken);

            return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Success, data.Select(x => new GetListVipOwnerOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}
