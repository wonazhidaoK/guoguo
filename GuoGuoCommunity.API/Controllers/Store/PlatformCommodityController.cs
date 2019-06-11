using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 平台商品库
    /// </summary>
    public class PlatformCommodityController : BaseController
    {
        private readonly IPlatformCommodityRepository _platformCommodityRepository;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        public PlatformCommodityController(IPlatformCommodityRepository platformCommodityRepository)
        {
            _platformCommodityRepository = platformCommodityRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加商品库
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("platformCommodity/add")]
        public async Task<ApiResult<AddBuildingOutput>> Add([FromBody]AddPlatformCommodityInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddBuildingOutput>(APIResultCode.Unknown, new AddBuildingOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("商品名称信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.BarCode))
            {
                throw new NotImplementedException("商品条形码信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddBuildingOutput>(APIResultCode.Unknown, new AddBuildingOutput { }, APIResultMessage.TokenError);
            }
            var dto = new PlatformCommodityDto
            {
                Name = input.Name,
                Price = input.Price,
                BarCode = input.BarCode,
                OperationTime = DateTimeOffset.Now,
                ImageUrl = input.ImageUrl,
                OperationUserId = user.Id.ToString()
            };

            var entity = await _platformCommodityRepository.AddAsync(dto, cancelToken);

            return new ApiResult<AddBuildingOutput>(APIResultCode.Success, new AddBuildingOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 删除平台商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("platformCommodity/delete")]
        public async Task<ApiResult> DeleteAsync([FromBody]DeletePlatformCommodity input, CancellationToken cancelToken)
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
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("平台商品ID无效！");
            }
            await _platformCommodityRepository.DeleteAsync(new PlatformCommodityDto
            {
                Id = input.Id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 分页查询所有平台商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("platformCommodity/getAllForPage")]
        public async Task<ApiResult<GetAllPlatformCommodityOutput>> GetListForPageAsync([FromBody]GetAllPlatformCommodityInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllPlatformCommodityOutput>(APIResultCode.Success_NoB, new GetAllPlatformCommodityOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllPlatformCommodityOutput>(APIResultCode.Unknown, new GetAllPlatformCommodityOutput { }, APIResultMessage.TokenNull);
            }

            var date = await _platformCommodityRepository.GetListForPageAsync(new PlatformCommodityDto
            {
                Name = input.Name,
                BarCode = input.BarCode,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            }, cancelToken);

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllPlatformCommodityOutput>(APIResultCode.Unknown, new GetAllPlatformCommodityOutput { }, APIResultMessage.TokenError);
            }
            List<GetPlatformCommodityOutput> listsss = new List<GetPlatformCommodityOutput>();
            foreach (var item in date.PlatformCommoditieForPageList)
            {
                listsss.Add(new GetPlatformCommodityOutput
                {
                    Id = item.Id.ToString(),
                    BarCode = item.BarCode,
                    Name = item.Name,
                    ImageUrl =  item.ImageUrl,
                    //ImageUrl = string.IsNullOrWhiteSpace(item.ImageUrl) ? "../Upload/icon-ts-mrpic.png" : item.ImageUrl,
                    Price = item.Price
                });
            }
            return new ApiResult<GetAllPlatformCommodityOutput>(APIResultCode.Success, new GetAllPlatformCommodityOutput
            {
                List = listsss,
                TotalCount = date.Count
            });
        }

        /// <summary>
        /// 根据ID查询平台商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("platformCommodity/get")]
        public async Task<ApiResult<GetPlatformCommodityOutput>> GetAsync([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetPlatformCommodityOutput>(APIResultCode.Unknown, new GetPlatformCommodityOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetPlatformCommodityOutput>(APIResultCode.Unknown, new GetPlatformCommodityOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new NotImplementedException("平台商品ID参数无效！");
            }
            var date = await _platformCommodityRepository.GetAsync(id, cancelToken);
            return new ApiResult<GetPlatformCommodityOutput>(APIResultCode.Success, new GetPlatformCommodityOutput
            {
                Id = id,
                Name = date.Name,
                ImageUrl = date.ImageUrl,
                BarCode = date.BarCode,
                Price = date.Price
            });
        }

        /// <summary>
        /// 更新平台商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("platformCommodity/update")]
        public async Task<ApiResult> UpdateAsync([FromBody]UpdatePlatformCommodityInput input, CancellationToken token = default)
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
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("平台商品ID参数无效！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("平台商品Name参数无效！");
            }

            if (input.Price < 0)
            {
                throw new NotImplementedException("平台商品Price参数无效！");
            }
            await _platformCommodityRepository.UpdateAsync(new PlatformCommodityDto
            {
                Id = input.Id,
                BarCode = input.BarCode,
                ImageUrl = input.ImageUrl,
                Name = input.Name,
                Price = input.Price,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, token);
            return new ApiResult();
        }

        /// <summary>
        /// 根据条形码查询平台商品
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("platformCommodity/getForBarCode")]
        public async Task<ApiResult<GetForBarCodePlatformCommodityOutput>> GetForBarCode([FromUri]string barCode, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetForBarCodePlatformCommodityOutput>(APIResultCode.Unknown, new GetForBarCodePlatformCommodityOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetForBarCodePlatformCommodityOutput>(APIResultCode.Unknown, new GetForBarCodePlatformCommodityOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(barCode))
            {
                throw new NotImplementedException("条形码为空！");
            }
            var data = await _platformCommodityRepository.GetForBarCodeAsync(barCode, cancelToken);
            if (data == null)
            {
                return new ApiResult<GetForBarCodePlatformCommodityOutput>(APIResultCode.Success, new GetForBarCodePlatformCommodityOutput { });
            }
            return new ApiResult<GetForBarCodePlatformCommodityOutput>(APIResultCode.Success, new GetForBarCodePlatformCommodityOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                BarCode = data.BarCode,
                ImageUrl = data.ImageUrl,
                Price = data.Price
            });
        }

    }
}
