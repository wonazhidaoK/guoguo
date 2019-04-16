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
    /// 小区
    /// </summary>
    public class SmallDistrictController : ApiController
    {
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smallDistrictRepository"></param>
        public SmallDistrictController(ISmallDistrictRepository smallDistrictRepository)
        {
            _smallDistrictRepository = smallDistrictRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加小区信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("smallDistrict/add")]
        public async Task<ApiResult<AddSmallDistrictOutput>> Add([FromBody]AddSmallDistrictInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddSmallDistrictOutput>(APIResultCode.Unknown, new AddSmallDistrictOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("小区名称信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Region))
                {
                    throw new NotImplementedException("小区区信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.State))
                {
                    throw new NotImplementedException("小区省信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.City))
                {
                    throw new NotImplementedException("小区城市信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
                {
                    throw new NotImplementedException("街道办Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.StreetOfficeName))
                {
                    throw new NotImplementedException("街道办信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.CommunityId))
                {
                    throw new NotImplementedException("小区Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.CommunityName))
                {
                    throw new NotImplementedException("小区信息为空！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddSmallDistrictOutput>(APIResultCode.Unknown, new AddSmallDistrictOutput { }, APIResultMessage.TokenError);
                }
                var entity = await _smallDistrictRepository.AddAsync(new SmallDistrictDto
                {
                    City = input.City,
                    Name = input.Name,
                    Region = input.Region,
                    State = input.State,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    CommunityId = input.CommunityId,
                    CommunityName = input.CommunityName,
                    StreetOfficeId = input.StreetOfficeId,
                    StreetOfficeName = input.StreetOfficeName
                }, cancelToken);

                return new ApiResult<AddSmallDistrictOutput>(APIResultCode.Success, new AddSmallDistrictOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddSmallDistrictOutput>(APIResultCode.Success_NoB, new AddSmallDistrictOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 修改小区信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("smallDistrict/update")]
        public async Task<ApiResult> Update([FromBody]UpdateSmallDistrictInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Id))
                {
                    throw new NotImplementedException("小区Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new NotImplementedException("小区名称信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _smallDistrictRepository.UpdateAsync(new SmallDistrictDto
                {
                    Id = input.Id,
                    Name = input.Name,
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
        /// 删除小区信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/delete")]
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
                    throw new NotImplementedException("小区Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _smallDistrictRepository.DeleteAsync(new SmallDistrictDto
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
        /// 获取小区详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/get")]
        public async Task<ApiResult<GetSmallDistrictOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("街道办Id信息为空！");
                }
                var data = await _smallDistrictRepository.GetAsync(id, cancelToken);

                return new ApiResult<GetSmallDistrictOutput>(APIResultCode.Success, new GetSmallDistrictOutput
                {
                    Id = data.Id.ToString(),
                    State = data.State,
                    City = data.City,
                    Region = data.Region,
                    Name = data.Name,
                    CommunityId = data.CommunityId,
                    CommunityName = data.CommunityName,
                    StreetOfficeId = data.StreetOfficeId,
                    StreetOfficeName = data.StreetOfficeName
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetSmallDistrictOutput>(APIResultCode.Success_NoB, new GetSmallDistrictOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询小区列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/getAll")]
        public async Task<ApiResult<GetAllSmallDistrictOutput>> GetAll([FromUri]GetAllSmallDistrictInput input, CancellationToken cancelToken)
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
                var data = await _smallDistrictRepository.GetAllAsync(new SmallDistrictDto
                {
                    Name = input?.Name,
                    City = input?.City,
                    State = input?.State,
                    Region = input?.Region,
                    CommunityId = input?.CommunityId,
                    StreetOfficeId = input?.StreetOfficeId
                }, cancelToken);

                var listCount = data.Count();
                var list = data.Skip(startRow).Take(input.PageSize);

                return new ApiResult<GetAllSmallDistrictOutput>(APIResultCode.Success, new GetAllSmallDistrictOutput
                {
                    List = list.Select(x => new GetSmallDistrictOutput
                    {
                        Id = x.Id.ToString(),
                        State = x.State,
                        City = x.City,
                        Region = x.Region,
                        Name = x.Name,
                        CommunityId = x.CommunityId,
                        CommunityName = x.CommunityName,
                        StreetOfficeId = x.StreetOfficeId,
                        StreetOfficeName = x.StreetOfficeName
                    }).ToList(),
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllSmallDistrictOutput>(APIResultCode.Success_NoB, new GetAllSmallDistrictOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 根据社区获取小区列表(小程序可用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/getList")]
        public async Task<ApiResult<List<GetListSmallDistrictOutput>>> GetList([FromUri]GetListSmallDistrictInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.CommunityId))
                {
                    throw new NotImplementedException("小区社区信息为空！");
                }

                var data = await _smallDistrictRepository.GetListAsync(new SmallDistrictDto
                {
                    CommunityId = input?.CommunityId
                }, cancelToken);

                return new ApiResult<List<GetListSmallDistrictOutput>>(APIResultCode.Success, data.Select(x => new GetListSmallDistrictOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListSmallDistrictOutput>>(APIResultCode.Success_NoB, new List<GetListSmallDistrictOutput> { }, e.Message);
            }
        }

        /// <summary>
        /// 根据街道办id查询小区列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/getAllForStreetOfficeId")]
        public async Task<ApiResult<GetAllSmallDistrictOutput>> GetAllForStreetOfficeId([FromUri]GetAllForStreetOfficeIdInput input, CancellationToken cancelToken)
        {
            try
            {
                var data = await _smallDistrictRepository.GetAllAsync(new SmallDistrictDto
                {
                    StreetOfficeId = input?.StreetOfficeId
                }, cancelToken);

                return new ApiResult<GetAllSmallDistrictOutput>(APIResultCode.Success, new GetAllSmallDistrictOutput
                {
                    List = data.Select(x => new GetSmallDistrictOutput
                    {
                        Id = x.Id.ToString(),
                        State = x.State,
                        City = x.City,
                        Region = x.Region,
                        Name = x.Name,
                        CommunityId = x.CommunityId,
                        CommunityName = x.CommunityName,
                        StreetOfficeId = x.StreetOfficeId,
                        StreetOfficeName = x.StreetOfficeName
                    }).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllSmallDistrictOutput>(APIResultCode.Success_NoB, new GetAllSmallDistrictOutput { }, e.Message);
            }
        }
    }
}
