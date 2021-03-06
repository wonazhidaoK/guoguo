﻿using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers.Store
{
    /// <summary>
    /// 购物车商品管理
    /// </summary>
    public class ShoppingTrollerController : BaseController
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IShoppingTrolleyRepository _shoppingTrolleyRepository;
        private readonly IShopCommodityRepository _shopCommodityRepository;

        /// <summary>
        /// 
        /// </summary>
        public ShoppingTrollerController(
            IShoppingTrolleyRepository shoppingTrolleyRepository,
            IShopCommodityRepository shopCommodityRepository,
            ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            _shoppingTrolleyRepository = shoppingTrolleyRepository;
            _shopCommodityRepository = shopCommodityRepository;
        }

        /// <summary>
        /// 新增购物车商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shoppingTroller/add")]
        public async Task<ApiResult<GetShoppingTrollerOutput>> Add([FromBody]AddShoppingTrollerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetShoppingTrollerOutput>(APIResultCode.Unknown, new GetShoppingTrollerOutput { }, APIResultMessage.TokenNull);
            }

            //if (string.IsNullOrWhiteSpace(input.OwnerCertificationRecordId))
            //{
            //    throw new NotImplementedException("业主ID参数无效！");
            //}
            if (string.IsNullOrWhiteSpace(input.ShopCommodityId))
            {
                throw new NotImplementedException("商品ID参数无效！");
            }
            if (input.CommodityCount <= 0)
            {
                throw new NotImplementedException("商品数量参数无效！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetShoppingTrollerOutput>(APIResultCode.Success_NoB, new GetShoppingTrollerOutput { }, "创建人信息不正确");
            }

            var result = await _shoppingTrolleyRepository.AddAsync(new ShoppingTrolleyDto
            {
                CommodityCount = input.CommodityCount,
                ShopCommodityId = input.ShopCommodityId,
                //OwnerCertificationRecordId = input.OwnerCertificationRecordId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            var shopCommodity = await _shopCommodityRepository.GetIncludeAsync(input.ShopCommodityId, cancelToken);

            var data = await _shoppingTrolleyRepository.GetAllIncludeAsync(new ShoppingTrolleyDto
            {
                // OwnerCertificationRecordId = input.OwnerCertificationRecordId,
                OperationUserId = user.Id.ToString(),
                ShopId = shopCommodity.GoodsType.ShopId.ToString()
            }, cancelToken);

            List<GetShoppingTrollerOutputModel> list = new List<GetShoppingTrollerOutputModel>();

            foreach (var item in data)
            {
                list.Add(new GetShoppingTrollerOutputModel
                {
                    CommodityCount = item.CommodityCount,
                    CommodityImageUrl = item.ShopCommodity.ImageUrl,
                    CommodityPrice = item.ShopCommodity.DiscountPrice,
                    CommodityId = item.ShopCommodityId.ToString(),
                    CommodityName = item.ShopCommodity.Name,
                    OriginalPrice = item.ShopCommodity.Price
                });
            }

            return new ApiResult<GetShoppingTrollerOutput>(APIResultCode.Success, new GetShoppingTrollerOutput
            {
                DiscountAmount = list.Sum(x => x.CommodityCount * x.OriginalPrice) - list.Sum(x => x.CommodityCount * x.CommodityPrice),
                Count = list.Sum(x => x.CommodityCount),
                Price = list.Sum(x => x.CommodityCount * x.CommodityPrice),
                List = list
            });
        }

        /// <summary>
        /// 清空购物车商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shoppingTroller/delete")]
        public async Task<ApiResult> Delete([FromBody]DeleteShopingTrollerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            //if (string.IsNullOrWhiteSpace(input.OwnerCertificationRecordId))
            //{
            //    throw new NotImplementedException("业主ID参数无效！");
            //}
            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                throw new NotImplementedException("店铺ID参数无效！");
            }
            var user = _tokenRepository.GetUser(Authorization);

            if (user == null)
            {
                return new ApiResult(APIResultCode.Success_NoB, "创建人信息不正确");
            }
            await _shoppingTrolleyRepository.DeleteAsync(new ShoppingTrolleyDto
            {
                // OwnerCertificationRecordId = input.OwnerCertificationRecordId,
                OperationUserId = user.Id.ToString(),
                ShopId = input.ShopId.ToString()
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 查询购物车商品列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shoppingTroller/getAll")]
        public async Task<ApiResult<GetAllShoppingTrollerOutput>> GetAll([FromUri]GetAllShoppingTrollerInput input, CancellationToken cancelToken)
        {
            //if (string.IsNullOrWhiteSpace(input.OwnerCertificationRecordId))
            //{
            //    throw new NotImplementedException("业主ID参数无效！");
            //}
            if (Authorization == null)
            {
                return new ApiResult<GetAllShoppingTrollerOutput>(APIResultCode.Unknown, new GetAllShoppingTrollerOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                throw new NotImplementedException("店铺ID参数无效！");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllShoppingTrollerOutput>(APIResultCode.Success_NoB, new GetAllShoppingTrollerOutput { }, "创建人信息不正确");
            }
            var data = await _shoppingTrolleyRepository.GetAllIncludeAsync(new ShoppingTrolleyDto
            {
                //OwnerCertificationRecordId = input.OwnerCertificationRecordId,
                OperationUserId = user.Id.ToString(),
                ShopId = input.ShopId
            }, cancelToken);

            List<GetAllShoppingTrollerOutputModel> list = new List<GetAllShoppingTrollerOutputModel>();
            foreach (var item in data)
            {
                list.Add(new GetAllShoppingTrollerOutputModel
                {
                    CommodityCount = item.CommodityCount,
                    CommodityImageUrl = item.ShopCommodity.ImageUrl,
                    CommodityPrice = item.ShopCommodity.DiscountPrice,
                    CommodityId = item.ShopCommodityId.ToString(),
                    CommodityName = item.ShopCommodity.Name,
                    OriginalPrice = item.ShopCommodity.Price
                });
            }
            return new ApiResult<GetAllShoppingTrollerOutput>(APIResultCode.Success, new GetAllShoppingTrollerOutput
            {
                DiscountAmount = list.Sum(x => x.CommodityCount * x.OriginalPrice) - list.Sum(x => x.CommodityCount * x.CommodityPrice),
                Count = list.Sum(x => x.CommodityCount),
                Price = list.Sum(x => x.CommodityCount * x.CommodityPrice),
                List = list
            });
        }

        /// <summary>
        /// 更新购物车商品数量(商品数量做减量变化)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shoppingTroller/update")]
        public async Task<ApiResult<GetShoppingTrollerOutput>> Update([FromBody]UpdateShoppingTrollerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetShoppingTrollerOutput>(APIResultCode.Unknown, new GetShoppingTrollerOutput { }, APIResultMessage.TokenNull);
            }

            //if (string.IsNullOrWhiteSpace(input.OwnerCertificationRecordId))
            //{
            //    throw new NotImplementedException("业主ID参数无效！");
            //}
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetShoppingTrollerOutput>(APIResultCode.Unknown, new GetShoppingTrollerOutput { }, APIResultMessage.TokenError);
            }
            await _shoppingTrolleyRepository.UpdateAsync(new ShoppingTrolleyDto
            {
                OperationUserId = user.Id.ToString(),
                //OwnerCertificationRecordId = input.OwnerCertificationRecordId,
                ShopCommodityId = input.ShopCommodityId,
                CommodityCount = input.CommodityCount
            }, cancelToken);
            var shopCommodity = await _shopCommodityRepository.GetIncludeAsync(input.ShopCommodityId, cancelToken);

            var data = await _shoppingTrolleyRepository.GetAllIncludeAsync(new ShoppingTrolleyDto
            {
                OperationUserId = user.Id.ToString(),
                //OwnerCertificationRecordId = input.OwnerCertificationRecordId,
                ShopId = shopCommodity.GoodsType.ShopId.ToString()
            }, cancelToken);

            List<GetShoppingTrollerOutputModel> list = new List<GetShoppingTrollerOutputModel>();

            foreach (var item in data)
            {
                list.Add(new GetShoppingTrollerOutputModel
                {
                    CommodityCount = item.CommodityCount,
                    CommodityImageUrl = item.ShopCommodity.ImageUrl,
                    CommodityPrice = item.ShopCommodity.DiscountPrice,
                    CommodityId = item.ShopCommodityId.ToString(),
                    CommodityName = item.ShopCommodity.Name,
                    OriginalPrice = item.ShopCommodity.Price
                });
            }

            return new ApiResult<GetShoppingTrollerOutput>(APIResultCode.Success, new GetShoppingTrollerOutput
            {
                DiscountAmount = list.Sum(x => x.CommodityCount * x.OriginalPrice) - list.Sum(x => x.CommodityCount * x.CommodityPrice),
                Count = list.Sum(x => x.CommodityCount),
                Price = list.Sum(x => x.CommodityCount * x.CommodityPrice),
                List = list
            });
        }
    }
}
