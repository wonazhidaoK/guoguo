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
    /// 店铺商品分类
    /// </summary>
    public class GoodsTypeController : BaseController
    {
        private readonly TokenManager _tokenManager;
        private readonly IGoodsTypeRepository _goodsTypeRepository;

        /// <summary>
        /// 
        /// </summary>
        public GoodsTypeController(IGoodsTypeRepository goodsTypeRepository)
        {
            _tokenManager = new TokenManager();
            _goodsTypeRepository = goodsTypeRepository;
        }

        /// <summary>
        /// 新增商品分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("goodsType/add")]
        public async Task<ApiResult<AddGoodsTypeOutput>> Add([FromBody]AddGoodsTypeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddGoodsTypeOutput>(APIResultCode.Unknown, new AddGoodsTypeOutput { }, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("分类名称为空！");
            }
            if (input.Sort < 1 && input.Sort > 10)
            {
                throw new NotImplementedException("排序值介于1-10！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddGoodsTypeOutput>(APIResultCode.Unknown, new AddGoodsTypeOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _goodsTypeRepository.AddAsync(new GoodsTypeDto
            {
                ShopId = user.ShopId.ToString(),
                Sort = input.Sort,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddGoodsTypeOutput>(APIResultCode.Success, new AddGoodsTypeOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 删除商品分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("goodsType/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("商品分类Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _goodsTypeRepository.DeleteAsync(new GoodsTypeDto
            {
                Id = id,
                ShopId = user.ShopId.ToString(),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("goodsType/update")]
        public async Task<ApiResult> Update([FromBody]UpdateGoodsTypeInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("分类名称为空！");
            }
            if (input.Sort < 1 && input.Sort > 10)
            {
                throw new NotImplementedException("排序值介于1-10！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _goodsTypeRepository.UpdateAsync(new GoodsTypeDto
            {
                Id = input.Id,
                ShopId = user.ShopId.ToString(),
                Sort = input.Sort,
                Name = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 分页查询所有商品分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("goodsType/getAllForPage")]
        public async Task<ApiResult<GetAllForPageGoodsTypeOutput>> GetListForPageAsync([FromUri]GetAllForPageGoodsTypeInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllForPageGoodsTypeOutput>(APIResultCode.Success_NoB, new GetAllForPageGoodsTypeOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllForPageGoodsTypeOutput>(APIResultCode.Unknown, new GetAllForPageGoodsTypeOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForPageGoodsTypeOutput>(APIResultCode.Unknown, new GetAllForPageGoodsTypeOutput { }, APIResultMessage.TokenError);
            }

            var date = await _goodsTypeRepository.GetAllIncludeForPageAsync(new GoodsTypeDto
            {
                Name = input.Name,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                ShopId = user.ShopId.ToString(),
            }, cancelToken);


            List<GetAllForPageGoodsTypeOutputModel> list = new List<GetAllForPageGoodsTypeOutputModel>();
            foreach (var item in date.List)
            {
                list.Add(new GetAllForPageGoodsTypeOutputModel
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Sort = item.Sort,
                    //PageIndex = input.PageIndex,
                    //PageSize = input.PageSize
                });
            }
            return new ApiResult<GetAllForPageGoodsTypeOutput>(APIResultCode.Success, new GetAllForPageGoodsTypeOutput
            {
                List = list,
                TotalCount = date.Count
            });
        }

        /// <summary>
        /// 根据商户Id获取商户类别
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("goodsType/getList")]
        public async Task<ApiResult<List<GetListGoodsTypeOutput>>> GetList([FromUri]string shopId, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListGoodsTypeOutput>>(APIResultCode.Unknown, new List<GetListGoodsTypeOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListGoodsTypeOutput>>(APIResultCode.Unknown, new List<GetListGoodsTypeOutput> { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(shopId))
            {
                throw new NotImplementedException("商户Id为空！");
            }
            var data = (await _goodsTypeRepository.GetListAsync(new GoodsTypeDto { ShopId = shopId }, cancelToken)).Select(x => new GetListGoodsTypeOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList(); ;

            return new ApiResult<List<GetListGoodsTypeOutput>>(APIResultCode.Success, data);
        }
    }
}