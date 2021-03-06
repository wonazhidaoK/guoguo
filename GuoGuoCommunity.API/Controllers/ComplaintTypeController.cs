﻿using GuoGuoCommunity.API.Models;
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
    /// 投诉类型管理
    /// </summary>
    public class ComplaintTypeController : BaseController
    {
        private readonly IComplaintTypeRepository _complaintTypeRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public ComplaintTypeController(IComplaintTypeRepository complaintTypeRepository, ITokenRepository tokenRepository)
        {
            _complaintTypeRepository = complaintTypeRepository;
            _tokenRepository = tokenRepository;
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
            if (Authorization == null)
            {
                return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Unknown, new AddComplaintTypeOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("投诉类型名称信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.InitiatingDepartmentValue))
            {
                throw new NotImplementedException("投诉类型发起部门值信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Level))
            {
                throw new NotImplementedException("投诉类型级别信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Unknown, new AddComplaintTypeOutput { }, APIResultMessage.TokenError);
            }
            var department = Department.GetAll().Where(x => x.Value == input.InitiatingDepartmentValue).FirstOrDefault();
            if (department == null)
            {
                throw new NotImplementedException("投诉类型发起部门信息不准确！");
            }
            var entity = await _complaintTypeRepository.AddAsync(new ComplaintTypeDto
            {
                Name = input.Name,
                Description = input.Description,
                Level = input.Level,
                InitiatingDepartmentName = department.Name,
                InitiatingDepartmentValue = input.InitiatingDepartmentValue,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult<AddComplaintTypeOutput>(APIResultCode.Success, new AddComplaintTypeOutput { Id = entity.Id.ToString() });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇单元Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _complaintTypeRepository.DeleteAsync(new ComplaintTypeDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _complaintTypeRepository.UpdateAsync(new ComplaintTypeDto
            {
                Id = input.Id,
                Description = input.Description,
                Level = input.Level,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancellationToken);

            return new ApiResult();
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
            if (Authorization == null)
            {
                return new ApiResult<GetComplaintTypeOutput>(APIResultCode.Unknown, new GetComplaintTypeOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetComplaintTypeOutput>(APIResultCode.Unknown, new GetComplaintTypeOutput { }, APIResultMessage.TokenError);
            }
            var data = await _complaintTypeRepository.GetAsync(id, cancelToken);

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
            if (Authorization == null)
            {
                return new ApiResult<GetAllComplaintTypeOutput>(APIResultCode.Unknown, new GetAllComplaintTypeOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllComplaintTypeOutput>(APIResultCode.Unknown, new GetAllComplaintTypeOutput { }, APIResultMessage.TokenError);
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

            var data = await _complaintTypeRepository.GetAllAsync(new ComplaintTypeDto
            {
                Description = input.Description,
                Name = input.Name,
                InitiatingDepartmentValue = input.InitiatingDepartmentValue
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllComplaintTypeOutput>(APIResultCode.Success, new GetAllComplaintTypeOutput
            {
                List = list.Select(x => new GetComplaintTypeOutput
                {
                    Id = x.Id.ToString(),
                    ComplaintPeriod = x.ComplaintPeriod,
                    Description = x.Description,
                    InitiatingDepartmentValue = x.InitiatingDepartmentValue,
                    InitiatingDepartmentName = x.InitiatingDepartmentName,
                    Level = x.Level,
                    Name = x.Name,
                    ProcessingPeriod = x.ProcessingPeriod
                }).ToList(),
                TotalCount = listCount
            });

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
            if (Authorization == null)
            {
                return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Unknown, new List<GetListComplaintTypeOutput> { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input?.InitiatingDepartmentValue))
            {
                throw new NotImplementedException("发起部门值信息为空！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Unknown, new List<GetListComplaintTypeOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _complaintTypeRepository.GetListAsync(new ComplaintTypeDto
            {
                InitiatingDepartmentValue = input?.InitiatingDepartmentValue
            }, cancelToken);

            return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Success, data.Select(x => new GetListComplaintTypeOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("complaintType/getAllList")]
        public async Task<ApiResult<List<GetListComplaintTypeOutput>>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Unknown, new List<GetListComplaintTypeOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Unknown, new List<GetListComplaintTypeOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _complaintTypeRepository.GetListAsync(new ComplaintTypeDto
            {

            }, cancelToken);

            return new ApiResult<List<GetListComplaintTypeOutput>>(APIResultCode.Success, data.Select(x => new GetListComplaintTypeOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}
