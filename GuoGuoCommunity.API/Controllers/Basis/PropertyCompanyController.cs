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
    /// 物业公司管理
    /// </summary>
    public class PropertyCompanyController : BaseController
    {
        private readonly IPropertyCompanyRepository _propertyCompanyRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public PropertyCompanyController(IPropertyCompanyRepository propertyCompanyRepository, ITokenRepository tokenRepository)
        {
            _propertyCompanyRepository = propertyCompanyRepository;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// 添加物业公司
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("propertyCompany/add")]
        public async Task<ApiResult<AddPropertyCompanyOutput>> Add([FromBody]AddPropertyCompanyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddPropertyCompanyOutput>(APIResultCode.Unknown, new AddPropertyCompanyOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("物业名称信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Phone))
            {
                throw new NotImplementedException("物业电话信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Address))
            {
                throw new NotImplementedException("物业地址信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddPropertyCompanyOutput>(APIResultCode.Unknown, new AddPropertyCompanyOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _propertyCompanyRepository.AddAsync(new PropertyCompanyDto
            {
                Name = input.Name,
                Address = input.Address,
                Phone = input.Phone,
                Description = input.Description,
                LogoImageUrl = input.LogoImageUrl,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddPropertyCompanyOutput>(APIResultCode.Success, new AddPropertyCompanyOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 修改物业公司
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("propertyCompany/update")]
        public async Task<ApiResult> Update([FromBody]UpdatePropertyCompanyInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("物业公司Id为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("物业公司名称为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Address))
            {
                throw new NotImplementedException("物业地址信息为空！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _propertyCompanyRepository.UpdateAsync(new PropertyCompanyDto
            {
                Id = input.Id,
                Description = input.Description,
                LogoImageUrl = input.LogoImageUrl,
                Phone = input.Phone,
                Address = input.Address,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 删除物业公司
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("propertyCompany/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("商品Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _propertyCompanyRepository.DeleteAsync(new PropertyCompanyDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 分页查询所有物业公司
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("propertyCompany/getAllForPage")]
        public async Task<ApiResult<GetAllPropertyCompanyForPageOutput>> GetListForPageAsync([FromUri]GetAllPropertyCompanyForPageInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllPropertyCompanyForPageOutput>(APIResultCode.Success_NoB, new GetAllPropertyCompanyForPageOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllPropertyCompanyForPageOutput>(APIResultCode.Unknown, new GetAllPropertyCompanyForPageOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllPropertyCompanyForPageOutput>(APIResultCode.Unknown, new GetAllPropertyCompanyForPageOutput { }, APIResultMessage.TokenError);
            }

            var date = await _propertyCompanyRepository.GetListForPageAsync(new PropertyCompanyDto
            {
                Name = input.Name,
                Phone = input.Phone,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            }, cancelToken);


            List<GetAllPropertyCompanyForPageOutputModel> list = new List<GetAllPropertyCompanyForPageOutputModel>();
            foreach (var item in date.List)
            {
                list.Add(new GetAllPropertyCompanyForPageOutputModel
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Phone = item.Phone,
                    Address = item.Address,
                    CreateTime = item.CreateOperationTime.Value
                });
            }
            return new ApiResult<GetAllPropertyCompanyForPageOutput>(APIResultCode.Success, new GetAllPropertyCompanyForPageOutput
            {
                List = list,
                TotalCount = date.Count
            });
        }

        /// <summary>
        /// 获取物业公司详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("propertyCompany/get")]
        public async Task<ApiResult<GetPropertyCompanyOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetPropertyCompanyOutput>(APIResultCode.Unknown, new GetPropertyCompanyOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetPropertyCompanyOutput>(APIResultCode.Unknown, new GetPropertyCompanyOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("物业公司Id信息为空！");
            }
            var data = await _propertyCompanyRepository.GetAsync(id, cancelToken);

            return new ApiResult<GetPropertyCompanyOutput>(APIResultCode.Success, new GetPropertyCompanyOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                Address = data.Address,
                Phone = data.Phone,
                LogoImageUrl = data.LogoImageUrl,
                CreateTime = data.CreateOperationTime.Value,
                Description = data.Description
            });
        }

        /// <summary>
        /// 获取所有物业公司
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("propertyCompany/getList")]
        public async Task<ApiResult<List<GetListPropertyCompanyOutput>>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListPropertyCompanyOutput>>(APIResultCode.Unknown, new List<GetListPropertyCompanyOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListPropertyCompanyOutput>>(APIResultCode.Unknown, new List<GetListPropertyCompanyOutput> { }, APIResultMessage.TokenError);
            }

            var data = (await _propertyCompanyRepository.GetListAsync(new PropertyCompanyDto { }, cancelToken)).Select(x => new GetListPropertyCompanyOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList(); ;

            return new ApiResult<List<GetListPropertyCompanyOutput>>(APIResultCode.Success, data);
        }
    }
}