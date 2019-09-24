using GuoGuoCommunity.API.Models;
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
    /// 街道办
    /// </summary>
    public class StreetOfficeController : BaseController
    {
        private readonly IStreetOfficeRepository _streetOfficeRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 街道办
        /// </summary>
        public StreetOfficeController(IStreetOfficeRepository streetOfficeRepository, ITokenRepository tokenRepository)
        {
            _streetOfficeRepository = streetOfficeRepository;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// 添加街道办信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("streetOffice/add")]
        public async Task<ApiResult<AddStreetOfficeOutput>> Add([FromBody]AddStreetOfficeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddStreetOfficeOutput>(APIResultCode.Unknown, new AddStreetOfficeOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("街道办名称信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Region))
            {
                throw new NotImplementedException("街道办区信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.State))
            {
                throw new NotImplementedException("街道办省信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.City))
            {
                throw new NotImplementedException("街道办城市信息为空！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddStreetOfficeOutput>(APIResultCode.Unknown, new AddStreetOfficeOutput { }, APIResultMessage.TokenError);
            }
            var entity = await _streetOfficeRepository.AddAsync(new StreetOfficeDto
            {
                City = input.City,
                Name = input.Name,
                Region = input.Region,
                State = input.State,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddStreetOfficeOutput>(APIResultCode.Success, new AddStreetOfficeOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 修改街道办信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("streetOffice/update")]
        public async Task<ApiResult> Update([FromBody]UpdateStreetOfficeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("街道办Id信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("街道办名称信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _streetOfficeRepository.UpdateAsync(new StreetOfficeDto
            {
                Id = input.Id,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 删除街道办信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("streetOffice/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("街道办Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _streetOfficeRepository.DeleteAsync(new StreetOfficeDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 获取街道办详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("streetOffice/get")]
        public async Task<ApiResult<GetStreetOfficeOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetStreetOfficeOutput>(APIResultCode.Unknown, new GetStreetOfficeOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("街道办Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetStreetOfficeOutput>(APIResultCode.Unknown, new GetStreetOfficeOutput { }, APIResultMessage.TokenError);
            }
            var data = await _streetOfficeRepository.GetAsync(id, cancelToken);

            return new ApiResult<GetStreetOfficeOutput>(APIResultCode.Success, new GetStreetOfficeOutput
            {
                Id = data.Id.ToString(),
                State = data.State,
                City = data.City,
                Region = data.Region,
                Name = data.Name
            });
        }

        /// <summary>
        /// 查询街道办列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("streetOffice/getAll")]
        public async Task<ApiResult<GetAllStreetOfficeOutput>> GetAll([FromUri]GetAllStreetOfficeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllStreetOfficeOutput>(APIResultCode.Unknown, new GetAllStreetOfficeOutput { }, APIResultMessage.TokenNull);
            }
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllStreetOfficeOutput>(APIResultCode.Unknown, new GetAllStreetOfficeOutput { }, APIResultMessage.TokenError);
            }
            int startRow = (input.PageIndex - 1) * input.PageSize;
            var data = await _streetOfficeRepository.GetAllAsync(new StreetOfficeDto
            {
                Name = input?.Name,
                City = input?.City,
                State = input?.State,
                Region = input?.Region
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllStreetOfficeOutput>(APIResultCode.Success, new GetAllStreetOfficeOutput
            {
                List = list.Select(x => new GetStreetOfficeOutput
                {
                    Id = x.Id.ToString(),
                    State = x.State,
                    City = x.City,
                    Region = x.Region,
                    Name = x.Name
                }).ToList(),
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 根据区获取街道办列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("streetOffice/getList")]
        public async Task<ApiResult<List<GetListStreetOfficeOutput>>> GetList([FromUri]GetListStreetOfficeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListStreetOfficeOutput>>(APIResultCode.Unknown, new List<GetListStreetOfficeOutput> { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Region))
            {
                throw new NotImplementedException("街道办区信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.City))
            {
                throw new NotImplementedException("街道办城市信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.State))
            {
                throw new NotImplementedException("街道办省信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListStreetOfficeOutput>>(APIResultCode.Unknown, new List<GetListStreetOfficeOutput> { }, APIResultMessage.TokenError);
            }
            var data = await _streetOfficeRepository.GetListAsync(new StreetOfficeDto
            {
                Region = input?.Region,
                City = input?.City,
                State = input?.State
            }, cancelToken);

            return new ApiResult<List<GetListStreetOfficeOutput>>(APIResultCode.Success, data.Select(x => new GetListStreetOfficeOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList());
        }
    }
}
