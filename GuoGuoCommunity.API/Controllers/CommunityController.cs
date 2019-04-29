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
    /// 社区
    /// </summary>
    public class CommunityController : BaseController
    {
        private readonly ICommunityRepository _communityRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 社区
        /// </summary>
        /// <param name="communityRepository"></param>
        public CommunityController(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
            _tokenManager = new TokenManager();
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

            if (Authorization == null)
            {
                return new ApiResult<AddCommunityOutput>(APIResultCode.Unknown, new AddCommunityOutput { }, APIResultMessage.TokenNull);
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
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddCommunityOutput>(APIResultCode.Unknown, new AddCommunityOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _communityRepository.AddAsync(new CommunityDto
            {
                City = input.City,
                Name = input.Name,
                Region = input.Region,
                State = input.State,
                StreetOfficeId = input.StreetOfficeId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddCommunityOutput>(APIResultCode.Success, new AddCommunityOutput { Id = entity.Id.ToString() });
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

            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("社区名称信息为空！");
            }
            await _communityRepository.UpdateAsync(new CommunityDto
            {
                Id = input.Id,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });

            return new ApiResult();

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
            await _communityRepository.DeleteAsync(new CommunityDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
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
                if (Authorization == null)
                {
                    return new ApiResult<GetCommunityOutput>(APIResultCode.Unknown, new GetCommunityOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("社区Id信息为空！");
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<GetCommunityOutput>(APIResultCode.Unknown, new GetCommunityOutput { }, APIResultMessage.TokenError);
                }
                var data = await _communityRepository.GetAsync(id, cancelToken);

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
            if (Authorization == null)
            {
                return new ApiResult<GetAllCommunityOutput>(APIResultCode.Unknown, new GetAllCommunityOutput { }, APIResultMessage.TokenNull);
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
            var data = await _communityRepository.GetAllAsync(new CommunityDto
            {
                Name = input?.Name,
                City = input?.City,
                State = input?.State,
                Region = input?.Region,
                StreetOfficeId = input?.StreetOfficeId
            }, cancelToken);
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllCommunityOutput>(APIResultCode.Unknown, new GetAllCommunityOutput { }, APIResultMessage.TokenError);
            }
            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllCommunityOutput>(APIResultCode.Success, new GetAllCommunityOutput
            {
                List = list.Select(x => new GetCommunityOutput
                {
                    Id = x.Id.ToString(),
                    State = x.State,
                    City = x.City,
                    Region = x.Region,
                    Name = x.Name,
                    StreetOfficeId = x.StreetOfficeId,
                    StreetOfficeName = x.StreetOfficeName
                }).ToList(),
                TotalCount = listCount
            });

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
            if (Authorization == null)
            {
                return new ApiResult<List<GetListCommunityOutput>>(APIResultCode.Unknown, new List<GetListCommunityOutput> { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
            {
                throw new NotImplementedException("街道办区信息为空！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListCommunityOutput>>(APIResultCode.Unknown, new List<GetListCommunityOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _communityRepository.GetListAsync(new CommunityDto
            {
                StreetOfficeId = input?.StreetOfficeId
            }, cancelToken);

            return new ApiResult<List<GetListCommunityOutput>>(APIResultCode.Success, data.Select(x => new GetListCommunityOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}