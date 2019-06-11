using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 商户商品管理
    /// </summary>
    public class ShopCommodityController : BaseController
    {
        private readonly IShopCommodityRepository _shopCommodityRepository;
        private readonly IShoppingTrolleyRepository _shoppingTrolleyRepository;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopCommodityRepository"></param>
        /// <param name="shoppingTrolleyRepository"></param>
        public ShopCommodityController(
            IShopCommodityRepository shopCommodityRepository,
            IShoppingTrolleyRepository shoppingTrolleyRepository)
        {
            _shopCommodityRepository = shopCommodityRepository;
            _shoppingTrolleyRepository = shoppingTrolleyRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 新增商户商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shopCommodity/add")]
        public async Task<ApiResult<AddShopCommodityOutput>> Add([FromBody]AddShopCommodityInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddShopCommodityOutput>(APIResultCode.Unknown, new AddShopCommodityOutput { }, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(input.BarCode))
            {
                throw new NotImplementedException("商品条形码为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("商品名称为空！");
            }
            if (input.Price == default)
            {
                throw new NotImplementedException("商品原价价格不规范！");
            }
            if (input.DiscountPrice == default)
            {
                throw new NotImplementedException("商品销售价格不规范！");
            }
            if (input.Price< input.DiscountPrice)
            {
                throw new NotImplementedException("销售价格即促销价应小于等于商品原价");
            }
            if (input.Sort < 1 && input.Sort > 10)
            {
                throw new NotImplementedException("排序值介于1-10！");
            }
            if (string.IsNullOrWhiteSpace(input.TypeId))
            {
                throw new NotImplementedException("商品类别为空！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddShopCommodityOutput>(APIResultCode.Unknown, new AddShopCommodityOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _shopCommodityRepository.AddAsync(new ShopCommodityDto
            {
                BarCode = input.BarCode,
                CommodityStocks = input.CommodityStocks,
                Description = input.Description,
                DiscountPrice = input.DiscountPrice,
                ImageUrl = input.ImageUrl,
                Price = input.Price,
                TypeId = input.TypeId,
                SalesTypeName = SalesType.Shelf.Name,
                SalesTypeValue = SalesType.Shelf.Value,
                Sort = input.Sort,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddShopCommodityOutput>(APIResultCode.Success, new AddShopCommodityOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 修改商户商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shopCommodity/update")]
        public async Task<ApiResult> Update([FromBody]UpdateShopCommodityInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("商品Id为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("商品名称为空！");
            }
            if (input.Price == default)
            {
                throw new NotImplementedException("商品原价价格不规范！");
            }
            if (input.DiscountPrice == default)
            {
                throw new NotImplementedException("商品销售价格不规范！");
            }
            if (input.Price < input.DiscountPrice)
            {
                throw new NotImplementedException("销售价格即促销价应小于等于商品原价");
            }
            if (input.Sort < 1 && input.Sort > 10)
            {
                throw new NotImplementedException("排序值介于1-10！");
            }
            if (string.IsNullOrWhiteSpace(input.TypeId))
            {
                throw new NotImplementedException("商品类别为空！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _shopCommodityRepository.UpdateAsync(new ShopCommodityDto
            {
                Id = input.Id,
                CommodityStocks = input.CommodityStocks,
                Description = input.Description,
                DiscountPrice = input.DiscountPrice,
                ImageUrl = input.ImageUrl,
                Price = input.Price,
                TypeId = input.TypeId,
                Sort = input.Sort,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 删除商户商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopCommodity/delete")]
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

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _shopCommodityRepository.DeleteAsync(new ShopCommodityDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 分页查询所有商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopCommodity/getAllForPage")]
        public async Task<ApiResult<GetAllForPageShopCommodityOutput>> GetListForPage([FromUri]GetAllForPageShopCommodityInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllForPageShopCommodityOutput>(APIResultCode.Success_NoB, new GetAllForPageShopCommodityOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllForPageShopCommodityOutput>(APIResultCode.Unknown, new GetAllForPageShopCommodityOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForPageShopCommodityOutput>(APIResultCode.Unknown, new GetAllForPageShopCommodityOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                return new ApiResult<GetAllForPageShopCommodityOutput>(APIResultCode.Success_NoB, new GetAllForPageShopCommodityOutput { }, "商店Id为空");
            }
            var date = await _shopCommodityRepository.GetAllIncludeForPageAsync(new ShopCommodityDto
            {
                Name = input.Name,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                BarCode = input.BarCode,
                TypeId = input.TypeId,
                SalesTypeValue = input.SalesTypeValue,
                ShopId = input.ShopId
            }, cancelToken);


            List<GetAllForPageShopCommodityOutputModel> list = new List<GetAllForPageShopCommodityOutputModel>();
            foreach (var item in date.List)
            {
                list.Add(new GetAllForPageShopCommodityOutputModel
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Sort = item.Sort,
                    SalesTypeValue = item.SalesTypeValue,
                    SalesTypeName = item.SalesTypeName,
                    BarCode = item.BarCode,
                    CreateTime = item.CreateOperationTime.Value,
                    TypeId = item.TypeId.ToString(),
                    TypeName = item.GoodsType.Name
                });
            }
            return new ApiResult<GetAllForPageShopCommodityOutput>(APIResultCode.Success, new GetAllForPageShopCommodityOutput
            {
                List = list,
                TotalCount = date.Count
            });
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopCommodity/get")]
        public async Task<ApiResult<GetShopCommodityOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetShopCommodityOutput>(APIResultCode.Unknown, new GetShopCommodityOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetShopCommodityOutput>(APIResultCode.Unknown, new GetShopCommodityOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("商户Id信息为空！");
            }
            var data = await _shopCommodityRepository.GetIncludeAsync(id, cancelToken);

            return new ApiResult<GetShopCommodityOutput>(APIResultCode.Success, new GetShopCommodityOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                BarCode = data.BarCode,
                SalesTypeValue = data.SalesTypeValue,
                CommodityStocks = data.CommodityStocks,
                DiscountPrice = data.DiscountPrice,
                ImageUrl = data.ImageUrl,
                Price = data.Price,
                SalesTypeName = data.SalesTypeName,
                Sort = data.Sort,
                CreateTime = data.CreateOperationTime.Value,
                Description = data.Description,
                TypeId = data.TypeId.ToString(),
                TypeName = data.GoodsType.Name
            });
        }

        /// <summary>
        /// 商户商品上架
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shopCommodity/shelf")]
        public async Task<ApiResult> Shelf([FromBody]UpdateSalesTypeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("商品Id为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _shopCommodityRepository.UpdateSalesTypeAsync(new ShopCommodityDto
            {
                Id = input.Id,
                SalesTypeName = SalesType.Shelf.Name,
                SalesTypeValue = SalesType.Shelf.Value,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 商户商品下架
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shopCommodity/obtained")]
        public async Task<ApiResult> Obtained([FromBody]UpdateSalesTypeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("商品Id为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _shopCommodityRepository.UpdateSalesTypeAsync(new ShopCommodityDto
            {
                Id = input.Id,
                SalesTypeName = SalesType.Obtained.Name,
                SalesTypeValue = SalesType.Obtained.Value,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 分类查询(提供给小程序)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopCommodity/getListForShopUser")]
        public async Task<ApiResult<List<GetListForShopUserOutput>>> GetListForShopUser([FromUri]GetListForShopUserInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<List<GetListForShopUserOutput>>(APIResultCode.Success_NoB, new List<GetListForShopUserOutput> { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<List<GetListForShopUserOutput>>(APIResultCode.Unknown, new List<GetListForShopUserOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListForShopUserOutput>>(APIResultCode.Unknown, new List<GetListForShopUserOutput> { }, APIResultMessage.TokenError);
            }

            if (string.IsNullOrWhiteSpace(input.ApplicationRecordId))
            {
                return new ApiResult<List<GetListForShopUserOutput>>(APIResultCode.Success_NoB, new List<GetListForShopUserOutput> { }, "业主认证Id为空");
            }

            if (string.IsNullOrWhiteSpace(input.TypeId))
            {
                return new ApiResult<List<GetListForShopUserOutput>>(APIResultCode.Success_NoB, new List<GetListForShopUserOutput> { }, "商品类别Id为空");
            }

            var date = await _shopCommodityRepository.GetListAsync(new ShopCommodityDto
            {
                TypeId = input.TypeId
            }, cancelToken);

            List<GetListForShopUserOutput> list = new List<GetListForShopUserOutput>();

            foreach (var item in date)
            {
                var shoppingTrolley = await _shoppingTrolleyRepository.GetForShopCommodityIdAsync(
                    new ShoppingTrolleyDto
                    {
                        OwnerCertificationRecordId = input.ApplicationRecordId,
                        ShopCommodityId = item.Id.ToString()
                    });
                list.Add(new GetListForShopUserOutput
                {
                    CommodityId = item.Id.ToString(),
                    CommodityCount = shoppingTrolley != null ? shoppingTrolley.CommodityCount : 0,
                    CommodityImageUrl = item.ImageUrl,
                    CommodityName = item.Name,
                    CommodityPrice = item.Price,
                    DiscountPrice = item.DiscountPrice
                });
            }

            return new ApiResult<List<GetListForShopUserOutput>>(APIResultCode.Success, list);
        }
    }
}