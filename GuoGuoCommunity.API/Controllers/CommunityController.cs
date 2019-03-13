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
    /// 社区
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CommunityController : ApiController
    {
        private readonly ICommunityService _communityService;
        private TokenManager _tokenManager;

        /// <summary>
        /// 社区
        /// </summary>
        /// <param name="communityService"></param>
        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
            _tokenManager =new TokenManager();
        }

        /// <summary>
        /// 添加社区信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("community/add")]
        public async Task<ApiResult<AddCommunityOutput>> Add([FromBody]AddCommunityInput input, CancellationToken cancelToken)
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
                    throw new NotImplementedException("社区名称信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Region))
                {
                    throw new NotImplementedException("社区区信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.State))
                {
                    throw new NotImplementedException("社区省信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.City))
                {
                    throw new NotImplementedException("社区城市信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
                {
                    throw new NotImplementedException("街道办信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.StreetOfficeName))
                {
                    throw new NotImplementedException("街道办信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    throw new NotImplementedException("token无效！");
                }

                var entity = await _communityService.AddAsync(new CommunityDto
                {
                    City = input.City,
                    Name = input.Name,
                    Region = input.Region,
                    State = input.State,
                    StreetOfficeId = input.StreetOfficeId,
                    StreetOfficeName = input.StreetOfficeName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddCommunityOutput>(APIResultCode.Success, new AddCommunityOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddCommunityOutput>(APIResultCode.Success_NoB, new AddCommunityOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 修改社区信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("community/update")]
        public async Task<ApiResult> Update([FromBody]UpdateCommunityInput input, CancellationToken cancellationToken)
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

                await _communityService.UpdateAsync(new CommunityDto
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
        /// 删除社区信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("community/delete")]
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
                await _communityService.DeleteAsync(new CommunityDto
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
        /// 获取社区详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("community/get")]
        public async Task<ApiResult<GetCommunityOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("社区Id信息为空！");
                }
                var data = await _communityService.GetAsync(id, cancelToken);

                return new ApiResult<GetCommunityOutput>(APIResultCode.Success, new GetCommunityOutput
                {
                    Id = data.Id.ToString(),
                    State = data.State,
                    City = data.City,
                    Region = data.Region,
                    Name = data.Name,
                    StreetOfficeId = data.StreetOfficeId,
                    StreetOfficeName = data.StreetOfficeName
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetCommunityOutput>(APIResultCode.Success_NoB, new GetCommunityOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询社区列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("community/getAll")]
        public async Task<ApiResult<GetAllCommunityOutput>> GetAll([FromUri]GetAllCommunityInput input, CancellationToken cancelToken)
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
                var data = await _communityService.GetAllAsync(new CommunityDto
                {
                    Name = input?.Name,
                    City = input?.City,
                    State = input?.State,
                    Region = input?.Region,
                    StreetOfficeId = input?.StreetOfficeId
                }, cancelToken);

                return new ApiResult<GetAllCommunityOutput>(APIResultCode.Success, new GetAllCommunityOutput
                {
                    List = data.Select(x => new GetCommunityOutput
                    {
                        Id = x.Id.ToString(),
                        State = x.State,
                        City = x.City,
                        Region = x.Region,
                        Name = x.Name,
                        StreetOfficeId = x.StreetOfficeId,
                        StreetOfficeName = x.StreetOfficeName
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllCommunityOutput>(APIResultCode.Success_NoB, new GetAllCommunityOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据街道办获取社区(小程序可用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("community/getList")]
        public async Task<ApiResult<List<GetListCommunityOutput>>> GetList([FromUri]GetListCommunityInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
                {
                    throw new NotImplementedException("街道办区信息为空！");
                }

                var data = await _communityService.GetListAsync(new CommunityDto
                {
                     StreetOfficeId = input?.StreetOfficeId
                }, cancelToken);

                return new ApiResult<List<GetListCommunityOutput>>(APIResultCode.Success, data.Select(x => new GetListCommunityOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListCommunityOutput>>(APIResultCode.Success_NoB, new List<GetListCommunityOutput> { }, e.Message);
            }
        }
    }
}