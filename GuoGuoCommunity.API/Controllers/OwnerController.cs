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
    /// 业主信息管理
    /// </summary>
    public class OwnerController : BaseController
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
            if (Authorization == null)
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
            if ((await _ownerRepository.GetAllAsync(new OwnerDto
            {
                IndustryId = input.IndustryId
            })).Any())
            {
                throw new NotImplementedException("该业户下存在业主！暂不支持多业主，不能添加");
            }
            await _ownerRepository.GetAllAsync(new OwnerDto
            {
                IndustryId = input.IndustryId
            });
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddOwnerOutput>(APIResultCode.Unknown, new AddOwnerOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _ownerRepository.AddAsync(new OwnerDto
            {
                Name = input.Name,
                PhoneNumber = input.PhoneNumber,
                IndustryId = input.IndustryId,
                IDCard = input.IDCard,
                Gender = input.Gender,
                Birthday = input.Birthday,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddOwnerOutput>(APIResultCode.Success, new AddOwnerOutput { Id = entity.Id.ToString() });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("业主Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
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
                IDCard = input.IDCard,
                PhoneNumber = input.PhoneNumber,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });
            return new ApiResult();
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
            if (Authorization == null)
            {
                return new ApiResult<GetOwnerOutput>(APIResultCode.Unknown, new GetOwnerOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("业主Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetOwnerOutput>(APIResultCode.Unknown, new GetOwnerOutput { }, APIResultMessage.TokenError);
            }

            var data = await _ownerRepository.GetIncludeAsync(id, cancelToken);

            return new ApiResult<GetOwnerOutput>(APIResultCode.Success, new GetOwnerOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                Birthday = data.Birthday,
                Gender = data.Gender,
                IDCard = data.IDCard,
                IndustryId = data.IndustryId.ToString(),
                IndustryName = data.Industry.Name,
                PhoneNumber = data.PhoneNumber,
                IsLegalize = data.IsLegalize
            });
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllOwnerOutput>(APIResultCode.Unknown, new GetAllOwnerOutput { }, APIResultMessage.TokenNull);
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
            if (string.IsNullOrWhiteSpace(input.IndustryId))
            {
                throw new NotImplementedException("业户Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllOwnerOutput>(APIResultCode.Unknown, new GetAllOwnerOutput { }, APIResultMessage.TokenError);
            }
            var data = await _ownerRepository.GetAllIncludeAsync(new OwnerDto
            {
                Name = input?.Name,
                IDCard = input?.IDCard,
                IndustryId = input.IndustryId,
                PhoneNumber = input.PhoneNumber,
                Gender = input.Gender
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllOwnerOutput>(APIResultCode.Success, new GetAllOwnerOutput
            {
                List = list.Select(x => new GetOwnerOutput
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    IndustryId = x.IndustryId.ToString(),
                    IDCard = x.IDCard,
                    Birthday = x.Birthday,
                    IndustryName = x.Industry.Name,
                    IsLegalize = x.IsLegalize
                }).ToList(),
                TotalCount = listCount
            });
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
            if (Authorization == null)
            {
                return new ApiResult<List<GetListOwnerOutput>>(APIResultCode.Unknown, new List<GetListOwnerOutput> { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.IndustryId))
            {
                throw new NotImplementedException("业户Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListOwnerOutput>>(APIResultCode.Unknown, new List<GetListOwnerOutput> { }, APIResultMessage.TokenError);
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
    }
}
