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
    /// 投诉类型管理
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ComplaintTypeController : ApiController
    {
        private readonly IComplaintTypeService _complaintTypeService;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="complaintTypeService"></param>
        public ComplaintTypeController(IComplaintTypeService complaintTypeService)
        {
            _complaintTypeService = complaintTypeService;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加投诉类型信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintType/add")]
        public async Task<ApiResult<AddComplaintTypeOutput>> Add([FromBody]AddComplaintTypeInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Unknown, new AddComplaintTypeOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("投诉类型名称信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.InitiatingDepartmentName))
                {
                    throw new NotImplementedException("投诉类型发起部门名称信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.InitiatingDepartmentValue))
                {
                    throw new NotImplementedException("投诉类型发起部门值信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Level))
                {
                    throw new NotImplementedException("投诉类型级别信息为空！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Unknown, new AddComplaintTypeOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _complaintTypeService.AddAsync(new ComplaintTypeDto
                {

                    Name = input.Name,
                    Description = input.Description,
                    Level = input.Level,
                    InitiatingDepartmentName = input.InitiatingDepartmentName,
                    InitiatingDepartmentValue = input.InitiatingDepartmentValue,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Success, new AddComplaintTypeOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Success_NoB, new AddComplaintTypeOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 删除投诉类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaintType/delete")]
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
                    throw new NotImplementedException("楼宇单元Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _complaintTypeService.DeleteAsync(new ComplaintTypeDto
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
        /// 修改投诉类型信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("complaintType/update")]
        public async Task<ApiResult> Update([FromBody]UpdateComplaintTypeInput input, CancellationToken cancellationToken)
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

                await _complaintTypeService.UpdateAsync(new ComplaintTypeDto
                {
                    Id = input.Id,
                    Description = input.Description,
                    InitiatingDepartmentName = input.InitiatingDepartmentName,
                    InitiatingDepartmentValue = input.InitiatingDepartmentValue,
                    Level = input.Level,
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
        /// 获取投诉类型详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaintType/get")]
        public async Task<ApiResult<GetComplaintTypeOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                var data = await _complaintTypeService.GetAsync(id, cancelToken);

                return new ApiResult<GetComplaintTypeOutput>(APIResultCode.Success, new GetComplaintTypeOutput
                {
                    Id = data.Id.ToString(),
                    ComplaintPeriod = data.ComplaintPeriod,
                    Description = data.Description,
                    InitiatingDepartmentName = data.InitiatingDepartmentName,
                    InitiatingDepartmentValue = data.InitiatingDepartmentValue,
                    Level = data.Level,
                    Name = data.Name,
                    ProcessingPeriod = data.ProcessingPeriod
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetComplaintTypeOutput>(APIResultCode.Success_NoB, new GetComplaintTypeOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询投诉类型列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaintType/getAll")]
        public async Task<ApiResult<GetAllComplaintTypeOutput>> GetAll([FromUri]GetAllComplaintTypeInput input, CancellationToken cancelToken)
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
                var data = await _complaintTypeService.GetAllAsync(new ComplaintTypeDto
                {
                    Description = input.Description,
                    Name = input.Name,
                    InitiatingDepartmentValue = input.InitiatingDepartmentValue
                }, cancelToken);

                return new ApiResult<GetAllComplaintTypeOutput>(APIResultCode.Success, new GetAllComplaintTypeOutput
                {
                    List = data.Select(x => new GetComplaintTypeOutput
                    {
                        Id = x.Id.ToString(),
                        ComplaintPeriod = x.ComplaintPeriod,
                        Description = x.Description,
                        InitiatingDepartmentValue = x.InitiatingDepartmentValue,
                        InitiatingDepartmentName = x.InitiatingDepartmentName,
                        Level = x.Level,
                        Name = x.Name,
                        ProcessingPeriod = x.ProcessingPeriod
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllComplaintTypeOutput>(APIResultCode.Success_NoB, new GetAllComplaintTypeOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据发起部门获取投诉类型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("complaintType/getList")]
        public async Task<ApiResult<List<GetListComplaintTypeOutput>>> GetList([FromUri]GetListComplaintTypeInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.InitiatingDepartmentValue))
                {
                    throw new NotImplementedException("发起部门值信息为空！");
                }

                var data = await _complaintTypeService.GetListAsync(new ComplaintTypeDto
                {
                    InitiatingDepartmentValue = input.InitiatingDepartmentValue
                }, cancelToken);

                return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Success, data.Select(x => new GetListComplaintTypeOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Success_NoB, new List<GetListComplaintTypeOutput> { }, e.Message);
            }
        }
    }
}
