using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 用户地址管理
    /// </summary>
    public class ShopUserAddressController : BaseController
    {
        private readonly TokenManager _tokenManager;
        private readonly IShopUserAddressRepository _shopUserAddressRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopUserAddressRepository"></param>
        public ShopUserAddressController(IShopUserAddressRepository shopUserAddressRepository)
        {
            _tokenManager = new TokenManager();
            _shopUserAddressRepository = shopUserAddressRepository;
        }

        /// <summary>
        /// 新增用户地址
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shopUserAddress/add")]
        public async Task<ApiResult<AddShopUserAddressOutput>> Add([FromBody]AddShopUserAddressInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddShopUserAddressOutput>(APIResultCode.Unknown, new AddShopUserAddressOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("收货人名称为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Phone))
            {
                throw new NotImplementedException("收货人电话为空！");
            }
            if (string.IsNullOrWhiteSpace(input.IndustryId))
            {
                throw new NotImplementedException("业户Id为空！");
            }
            if (string.IsNullOrWhiteSpace(input.ApplicationRecordId))
            {
                throw new NotImplementedException("业户认证申请Id为空！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddShopUserAddressOutput>(APIResultCode.Unknown, new AddShopUserAddressOutput { }, APIResultMessage.TokenError);
            }
            if (!(user.DepartmentValue == Department.YeZhu.Value || user.DepartmentValue == Department.YeZhuWeiYuanHui.Value))
            {
                throw new NotImplementedException("当前登陆人身份不可以维护地址！");
            }

            var entity = await _shopUserAddressRepository.AddAsync(new ShopUserAddressDto
            {
                IndustryId = input.IndustryId,
                ReceiverPhone = input.Phone,
                ApplicationRecordId = input.ApplicationRecordId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                ReceiverName = input.Name,
                IsDefault = input.IsDefault
            }, cancelToken);

            return new ApiResult<AddShopUserAddressOutput>(APIResultCode.Success, new AddShopUserAddressOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 删除用户地址信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopUserAddress/delete")]
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
            await _shopUserAddressRepository.DeleteAsync(new ShopUserAddressDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 修改用户地址
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shopUserAddress/update")]
        public async Task<ApiResult> Update([FromBody]UpdateShopUserAddressInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new NotImplementedException("分类名称为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Phone))
            {
                throw new NotImplementedException("收货人电话为空！");
            }
            if (string.IsNullOrWhiteSpace(input.IndustryId))
            {
                throw new NotImplementedException("业户Id为空！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _shopUserAddressRepository.UpdateAsync(new ShopUserAddressDto
            {
                Id = input.Id,
                IsDefault = input.IsDefault,
                IndustryId = input.IndustryId,
                ReceiverPhone = input.Phone,
                ReceiverName = input.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 根据业主申请Id获取地址列表
        /// </summary>
        /// <param name="applicationRecordId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopUserAddress/getList")]
        public async Task<ApiResult<List<GetListShopUserAddressOutput>>> GetList([FromUri]string applicationRecordId, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListShopUserAddressOutput>>(APIResultCode.Unknown, new List<GetListShopUserAddressOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListShopUserAddressOutput>>(APIResultCode.Unknown, new List<GetListShopUserAddressOutput> { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(applicationRecordId))
            {
                throw new NotImplementedException("业主申请Id为空！");
            }
            var data = (await _shopUserAddressRepository.GetAllIncludeAsync(new ShopUserAddressDto { ApplicationRecordId = applicationRecordId }, cancelToken)).Select(x => new GetListShopUserAddressOutput
            {
                Id = x.Id.ToString(),
                Name = x.ReceiverName,
                Phone = x.ReceiverPhone,
                IsDefault = x.IsDefault,
                Address = x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.State + x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.City +
              x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Region + x.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Name + x.Industry.BuildingUnit.Building.SmallDistrict.Community.Name + x.Industry.BuildingUnit.Building.SmallDistrict.Name
            }).ToList();
            if (data.Any())
            {
                //如果没有默认的地址第一条地址为默认
                if (!data.Where(x => x.IsDefault).Any())
                {
                    data[0].IsDefault = true;
                }
            }

            return new ApiResult<List<GetListShopUserAddressOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 获取地址详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shopUserAddress/get")]
        public async Task<ApiResult<GetShopUserAddressOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetShopUserAddressOutput>(APIResultCode.Unknown, new GetShopUserAddressOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetShopUserAddressOutput>(APIResultCode.Unknown, new GetShopUserAddressOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("地址Id信息为空！");
            }
            var data = await _shopUserAddressRepository.GetIncludeAsync(id, cancelToken);

            return new ApiResult<GetShopUserAddressOutput>(APIResultCode.Success, new GetShopUserAddressOutput
            {
                Id = data.Id.ToString(),
                Name = data.ReceiverName,
                //Address = data.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.State + data.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.City +
                //data.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Region + data.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Name + data.Industry.BuildingUnit.Building.SmallDistrict.Community.Name + data.Industry.BuildingUnit.Building.SmallDistrict.Name,
                Phone = data.ReceiverPhone,
                BuildingUnitId = data.Industry.BuildingUnitId.ToString(),
                BuildingId = data.Industry.BuildingUnit.BuildingId.ToString(),
                BuildingName = data.Industry.BuildingUnit.Building.Name,
                BuildingUnitName = data.Industry.BuildingUnit.UnitName,
                NumberOfLayers = data.Industry.NumberOfLayers,
                IndustryId = data.IndustryId.ToString(),
                IndustryName = data.Industry.Name,
                IsDefault = data.IsDefault
            });
        }
    }
}
