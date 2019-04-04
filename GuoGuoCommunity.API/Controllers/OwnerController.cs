﻿using GuoGuoCommunity.API.Models;
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
    /// 业主信息管理
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OwnerController : ApiController
    {
        private readonly IOwnerRepository _ownerRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerRepository"></param>
        public OwnerController(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加业主信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("owner/add")]
        public async Task<ApiResult<AddOwnerOutput>> Add([FromBody]AddOwnerInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddOwnerOutput>(APIResultCode.Unknown, new AddOwnerOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("业主姓名信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.Birthday))
                {
                    throw new NotImplementedException("业主生日信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.Gender))
                {
                    throw new NotImplementedException("业主性别信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.PhoneNumber))
                {
                    throw new NotImplementedException("业主手机号信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.IDCard))
                {
                    throw new NotImplementedException("业主身份证信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.IndustryId))
                {
                    throw new NotImplementedException("业主业户Id信息为空！");
                }
                //if (string.IsNullOrWhiteSpace(input.IndustryName))
                //{
                //    throw new NotImplementedException("楼宇小区名称信息为空！");
                //}

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddOwnerOutput>(APIResultCode.Unknown, new AddOwnerOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _ownerRepository.AddAsync(new OwnerDto
                {
                    Name = input.Name,
                    PhoneNumber = input.PhoneNumber,
                    // IndustryName = input.IndustryName,
                    IndustryId = input.IndustryId,
                    IDCard = input.IDCard,
                    Gender = input.Gender,
                    Birthday = input.Birthday,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddOwnerOutput>(APIResultCode.Success, new AddOwnerOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddOwnerOutput>(APIResultCode.Success_NoB, new AddOwnerOutput { }, e.Message);
            }
        }


        /// <summary>
        /// 删除业主信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("owner/delete")]
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
                    throw new NotImplementedException("业主Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _ownerRepository.DeleteAsync(new OwnerDto
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
        /// 修改业主信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("owner/update")]
        public async Task<ApiResult> Update([FromBody]UpdateOwnerInput input, CancellationToken cancellationToken)
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

                await _ownerRepository.UpdateAsync(new OwnerDto
                {
                    Id = input.Id,
                    Name = input.Name,
                    Birthday = input.Birthday,
                    Gender = input.Gender,
                    IDCard = input.Name,
                    PhoneNumber = input.PhoneNumber,
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
        /// 获取业主详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("owner/get")]
        public async Task<ApiResult<GetOwnerOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("业主Id信息为空！");
                }
                var data = await _ownerRepository.GetAsync(id, cancelToken);

                return new ApiResult<GetOwnerOutput>(APIResultCode.Success, new GetOwnerOutput
                {
                    Id = data.Id.ToString(),
                    Name = data.Name,
                    Birthday = data.Birthday,
                    Gender = data.Gender,
                    IDCard = data.IDCard,
                    IndustryId = data.IndustryId,
                    IndustryName = data.IndustryName,
                    PhoneNumber = data.PhoneNumber
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetOwnerOutput>(APIResultCode.Success_NoB, new GetOwnerOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询业主列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("owner/getAll")]
        public async Task<ApiResult<GetAllOwnerOutput>> GetAll([FromUri]GetAllOwnerInput input, CancellationToken cancelToken)
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
                var data = await _ownerRepository.GetAllAsync(new OwnerDto
                {
                    Name = input?.Name,
                    IDCard = input?.IDCard,
                    IndustryId = input.IndustryId,
                    PhoneNumber = input.PhoneNumber,
                    Gender = input.Gender
                }, cancelToken);

                return new ApiResult<GetAllOwnerOutput>(APIResultCode.Success, new GetAllOwnerOutput
                {
                    List = data.Select(x => new GetOwnerOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Gender = x.Gender,
                        PhoneNumber = x.PhoneNumber,
                        IndustryId = x.IndustryId,
                        IDCard = x.IDCard,
                        Birthday = x.Birthday,
                        IndustryName = x.IndustryName
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllOwnerOutput>(APIResultCode.Success_NoB, new GetAllOwnerOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据业户id获取业主
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("owner/getList")]
        public async Task<ApiResult<List<GetListOwnerOutput>>> GetList([FromUri]GetListOwnerInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.IndustryId))
                {
                    throw new NotImplementedException("业户Id信息为空！");
                }

                var data = await _ownerRepository.GetListAsync(new OwnerDto
                {
                    IndustryId = input.IndustryId
                }, cancelToken);

                return new ApiResult<List<GetListOwnerOutput>>(APIResultCode.Success, data.Select(x => new GetListOwnerOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListOwnerOutput>>(APIResultCode.Success_NoB, new List<GetListOwnerOutput> { }, e.Message);
            }
        }
    }
}
