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
    /// 业委会架构(职能)
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VipOwnerStructureController : ApiController
    {
        private readonly IVipOwnerStructureService _vipOwnerStructureService;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerStructureService"></param>
        public VipOwnerStructureController(IVipOwnerStructureService vipOwnerStructureService)
        {
            _vipOwnerStructureService = vipOwnerStructureService;
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    throw new NotImplementedException("token为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("职能名称信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.Weights))
                {
                    throw new NotImplementedException("职能权重信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    throw new NotImplementedException("token无效！");
                }

                var entity = await _vipOwnerStructureService.AddAsync(new VipOwnerStructureDto
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
            catch (Exception e)
            {
                return new ApiResult<AddVipOwnerStructureOutput>(APIResultCode.Success_NoB, new AddVipOwnerStructureOutput { }, e.Message);
            }
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

                await _vipOwnerStructureService.UpdateAsync(new VipOwnerStructureDto
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
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }

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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    throw new NotImplementedException("token信息为空！");
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("社区Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    throw new NotImplementedException("token无效！");
                }
                await _vipOwnerStructureService.DeleteAsync(new VipOwnerStructureDto
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
        /// 获取职能详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerStructure/get")]
        public async Task<ApiResult<GetVipOwnerStructureOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("社区Id信息为空！");
                }
                var data = await _vipOwnerStructureService.GetAsync(id, cancelToken);

                return new ApiResult<GetVipOwnerStructureOutput>(APIResultCode.Success, new GetVipOwnerStructureOutput
                {
                    Id = data.Id.ToString(),
                    Description = data.Description,
                    Name = data.Name,
                    IsReview = data.IsReview,
                    Weights = data.Weights
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetVipOwnerStructureOutput>(APIResultCode.Success_NoB, new GetVipOwnerStructureOutput { }, e.Message);
            }
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
                var data = await _vipOwnerStructureService.GetAllAsync(new VipOwnerStructureDto
                {
                    Name = input?.Name,
                    Weights = input.Weights,
                    IsReview = input.IsReview,
                    Description = input.Description
                }, cancelToken);

                return new ApiResult<GetAllVipOwnerStructureOutput>(APIResultCode.Success, new GetAllVipOwnerStructureOutput
                {
                    List = data.Select(x => new GetVipOwnerStructureOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Description = x.Description,
                        IsReview = x.IsReview,
                        Weights = x.Weights
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllVipOwnerStructureOutput>(APIResultCode.Success_NoB, new GetAllVipOwnerStructureOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 获取职能
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerStructure/getList")]
        public async Task<ApiResult<List<GetListVipOwnerStructureOutput>>> GetList(CancellationToken cancelToken)
        {
            try
            {
                var data = await _vipOwnerStructureService.GetListAsync(cancelToken);
                return new ApiResult<List<GetListVipOwnerStructureOutput>>(APIResultCode.Success, data.Select(x => new GetListVipOwnerStructureOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListVipOwnerStructureOutput>>(APIResultCode.Success_NoB, new List<GetListVipOwnerStructureOutput> { }, e.Message);
            }
        }
    }
}
