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

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 业委会管理
    /// </summary>
    public class VipOwnerController : ApiController
    {
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private TokenManager _tokenManager;

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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVipOwnerOutput>(APIResultCode.Unknown, new AddVipOwnerOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("业委会小区Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.SmallDistrictName))
                {
                    throw new NotImplementedException("业委会小区名称信息为空！");
                }

                //var data = await _vipOwnerRepository.GetListAsync(new VipOwnerDto
                //{
                //    SmallDistrictId = input.SmallDistrictId
                //}, cancelToken);
                //查询当前小区是否有为竞选业委会 如果有不允许创建
                if ((await _vipOwnerRepository.GetListAsync(new VipOwnerDto
                {
                    SmallDistrictId = input.SmallDistrictId
                }, cancelToken)).Any())
                {
                    throw new NotImplementedException("当前小区存在未竞选业委会！不能重复添加！");
                }
                //查询当前小区下所有非删除业委会
                var count = (await _vipOwnerRepository.GetIsValidAsync(new VipOwnerDto
                {
                    SmallDistrictId = input.SmallDistrictId
                })).Count;

                var user = _tokenManager.GetUser(token);

                if (user == null)
                {
                    return new ApiResult<AddVipOwnerOutput>(APIResultCode.Unknown, new AddVipOwnerOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _vipOwnerRepository.AddAsync(new VipOwnerDto
                {
                    Name = input.SmallDistrictName + "【第" + (count + 1) + "届】业主委员会",
                    RemarkName = input.RemarkName,
                    SmallDistrictId = input.SmallDistrictId,
                    SmallDistrictName = input.SmallDistrictName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddVipOwnerOutput>(APIResultCode.Success, new AddVipOwnerOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVipOwnerOutput>(APIResultCode.Success_NoB, new AddVipOwnerOutput { }, e.Message);
            }
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("业委会Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);

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
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("业委会Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);

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
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
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

                await _vipOwnerRepository.UpdateAsync(new VipOwnerDto
                {
                    Id = input.Id,
                    RemarkName = input.RemarkName,
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
        /// 获取业委会详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwner/get")]
        public async Task<ApiResult<GetVipOwnerOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                var data = await _vipOwnerRepository.GetAsync(id, cancelToken);

                return new ApiResult<GetVipOwnerOutput>(APIResultCode.Success, new GetVipOwnerOutput
                {
                    Id = data.Id.ToString(),
                    Name = data.Name,
                    IsValid = data.IsValid,
                    RemarkName = data.RemarkName,
                    SmallDistrictId = data.SmallDistrictId,
                    SmallDistrictName = data.SmallDistrictName
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetVipOwnerOutput>(APIResultCode.Success_NoB, new GetVipOwnerOutput { }, e.Message);
            }
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
                //if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                //{
                //    throw new NotImplementedException("业委会小区Id信息为空！");
                //}
                var data = await _vipOwnerRepository.GetAllAsync(new VipOwnerDto
                {
                    Name = input.Name,
                    //RemarkName = input.RemarkName,
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
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                        Name = x.Name,
                        IsElection = x.IsElection,
                        IsCanDeleted = !x.IsElection,
                        IsCanInvalid =  x.IsValid
                    }).ToList(),
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllVipOwnerOutput>(APIResultCode.Success_NoB, new GetAllVipOwnerOutput { }, e.Message);
            }
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
            try
            {
                if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息为空！");
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
            catch (Exception e)
            {
                return new ApiResult<List<GetListVipOwnerOutput>>(APIResultCode.Success_NoB, new List<GetListVipOwnerOutput> { }, e.Message);
            }
        }
    }
}
