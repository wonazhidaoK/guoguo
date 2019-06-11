using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
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
    /// 小区商户管理
    /// </summary>
    public class SmallDistrictShopController : BaseController
    {
        private readonly TokenManager _tokenManager;
        private readonly ISmallDistrictShopRepository _smallDistrictShopRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smallDistrictShopRepository"></param>
        /// <param name="shopRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        public SmallDistrictShopController(ISmallDistrictShopRepository smallDistrictShopRepository,
            IShopRepository shopRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository)
        {
            _tokenManager = new TokenManager();
            _smallDistrictShopRepository = smallDistrictShopRepository;
            _shopRepository = shopRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
        }

        /// <summary>
        /// 小区入住商户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("smallDistrictShop/add")]
        public async Task<ApiResult<AddSmallDistrictShopOutput>> Add([FromBody]AddSmallDistrictShopInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddSmallDistrictShopOutput>(APIResultCode.Unknown, new AddSmallDistrictShopOutput { }, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                throw new NotImplementedException("商户Id为空！");
            }
            if (input.Sort < 1 && input.Sort > 10)
            {
                throw new NotImplementedException("排序值介于1-10！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddSmallDistrictShopOutput>(APIResultCode.Unknown, new AddSmallDistrictShopOutput { }, APIResultMessage.TokenError);
            }
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                throw new NotImplementedException("操作人部门不为物业！");
            }
            if (string.IsNullOrWhiteSpace(user.SmallDistrictId))
            {
                throw new NotImplementedException("操作人小区信息为空！");
            }
            var entity = await _smallDistrictShopRepository.AddAsync(new SmallDistrictShopDto
            {
                ShopId = input.ShopId,
                Sort = input.Sort,
                Postage = input.Postage,
                SmallDistrictId = user.SmallDistrictId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddSmallDistrictShopOutput>(APIResultCode.Success, new AddSmallDistrictShopOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 修改入住商户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("smallDistrictShop/update")]
        public async Task<ApiResult> Update([FromBody]UpdateSmallDistrictShopInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("小区商户Id信息为空！");
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
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                throw new NotImplementedException("操作人部门不为物业！");
            }
            if (string.IsNullOrWhiteSpace(user.SmallDistrictId))
            {
                throw new NotImplementedException("操作人小区信息为空！");
            }

            await _smallDistrictShopRepository.UpdateAsync(new SmallDistrictShopDto
            {
                Id = input.Id,
                Sort = input.Sort,
                Postage = input.Postage,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 删除商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrictShop/delete")]
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
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                throw new NotImplementedException("操作人部门不为物业！");
            }
            if (string.IsNullOrWhiteSpace(user.SmallDistrictId))
            {
                throw new NotImplementedException("操作人小区信息为空！");
            }
            await _smallDistrictShopRepository.DeleteAsync(new SmallDistrictShopDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 分页查询小区内商店
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrictShop/getAllForPage")]
        public async Task<ApiResult<GetAllForPageSmallDistrictShopOutput>> GetListForPage([FromUri]GetAllForPageSmallDistrictShopInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllForPageSmallDistrictShopOutput>(APIResultCode.Success_NoB, new GetAllForPageSmallDistrictShopOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllForPageSmallDistrictShopOutput>(APIResultCode.Unknown, new GetAllForPageSmallDistrictShopOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForPageSmallDistrictShopOutput>(APIResultCode.Unknown, new GetAllForPageSmallDistrictShopOutput { }, APIResultMessage.TokenError);
            }
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                throw new NotImplementedException("操作人部门不为物业！");
            }
            if (string.IsNullOrWhiteSpace(user.SmallDistrictId))
            {
                throw new NotImplementedException("操作人小区信息为空！");
            }
            var date = await _smallDistrictShopRepository.GetAllIncludeForPageAsync(new SmallDistrictShopDto
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                SmallDistrictId = user.SmallDistrictId,
                ShopId = input.ShopId,

            }, cancelToken);


            List<GetAllForPageSmallDistrictShopOutputModel> list = new List<GetAllForPageSmallDistrictShopOutputModel>();
            foreach (var item in date.List)
            {
                list.Add(new GetAllForPageSmallDistrictShopOutputModel
                {
                    Id = item.Id.ToString(),
                    Name = item.Shop.Name,
                    Sort = item.Sort,
                    Postage = item.Postage,
                    CreateTime = item.CreateOperationTime.Value
                    //PageIndex = input.PageIndex,
                    //PageSize = input.PageSize
                });
            }
            return new ApiResult<GetAllForPageSmallDistrictShopOutput>(APIResultCode.Success, new GetAllForPageSmallDistrictShopOutput
            {
                List = list,
                TotalCount = date.Count
            });
        }

        /// <summary>
        /// 获取未选择商户
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrictShop/getListForNotSelected")]
        public async Task<ApiResult<List<GetListSmallDistrictShopOutput>>> GetListForNotSelected(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListSmallDistrictShopOutput>>(APIResultCode.Unknown, new List<GetListSmallDistrictShopOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListSmallDistrictShopOutput>>(APIResultCode.Unknown, new List<GetListSmallDistrictShopOutput> { }, APIResultMessage.TokenError);
            }
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                throw new NotImplementedException("操作人部门不为物业！");
            }
            if (string.IsNullOrWhiteSpace(user.SmallDistrictId))
            {
                throw new NotImplementedException("操作人小区信息为空！");
            }
            var list = (await _smallDistrictShopRepository.GetListAsync(new SmallDistrictShopDto { SmallDistrictId = user.SmallDistrictId }, cancelToken)).Select(x => x.ShopId.ToString()).ToList();

            var data = (await _shopRepository.GetListForNotIdsAsync(list, cancelToken)).Select(x => new GetListSmallDistrictShopOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList(); ;

            return new ApiResult<List<GetListSmallDistrictShopOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 小程序分页查询小区内商店
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrictShop/getAllForShopUser")]
        public async Task<ApiResult<GetAllForShopUserSmallDistrictShopOutput>> GetAllForShopUser([FromUri]GetAllForShopUserSmallDistrictShopInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllForShopUserSmallDistrictShopOutput>(APIResultCode.Success_NoB, new GetAllForShopUserSmallDistrictShopOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllForShopUserSmallDistrictShopOutput>(APIResultCode.Unknown, new GetAllForShopUserSmallDistrictShopOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForShopUserSmallDistrictShopOutput>(APIResultCode.Unknown, new GetAllForShopUserSmallDistrictShopOutput { }, APIResultMessage.TokenError);
            }
            if (!(user.DepartmentValue == Department.YeZhu.Value || user.DepartmentValue == Department.YeZhuWeiYuanHui.Value))
            {
                throw new NotImplementedException("操作人部门不为业主！");
            }
            //if (string.IsNullOrWhiteSpace(user.SmallDistrictId))
            //{
            //    throw new NotImplementedException("操作人小区信息为空！");
            //}
            var ownerCertificationRecord = await _ownerCertificationRecordRepository.GetIncludeAsync(input.ApplicationRecordId, cancelToken);
            if (ownerCertificationRecord == null)
            {
                throw new NotImplementedException("业主认证记录不存在为空！");
            }
            var date = await _smallDistrictShopRepository.GetAllIncludeForShopUserAsync(new SmallDistrictShopDto
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                SmallDistrictId = ownerCertificationRecord.Owner.Industry.BuildingUnit.Building.SmallDistrictId.ToString()

            }, cancelToken);


            List<GetAllForShopUserSmallDistrictShopOutputModel> list = new List<GetAllForShopUserSmallDistrictShopOutputModel>();
            foreach (var item in date.List)
            {
                list.Add(new GetAllForShopUserSmallDistrictShopOutputModel
                {
                    Id = item.Id.ToString(),
                    Name = item.Shop.Name,
                    Address = item.Shop.Address,
                    LogoImageUrl = item.Shop.LogoImageUrl,
                    PhoneNumber = item.Shop.PhoneNumber,
                    ShopId = item.Shop.Id.ToString()
                });
            }
            return new ApiResult<GetAllForShopUserSmallDistrictShopOutput>(APIResultCode.Success, new GetAllForShopUserSmallDistrictShopOutput
            {
                List = list,
                TotalCount = date.Count
            });
        }


        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrictShop/getList")]
        public async Task<ApiResult<dynamic>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<dynamic>(APIResultCode.Unknown, new { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<dynamic>(APIResultCode.Unknown, new { }, APIResultMessage.TokenError);
            }
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                throw new NotImplementedException("操作人部门不为物业！");
            }

            var date = (await _smallDistrictShopRepository.GetAllIncludeAsync(new SmallDistrictShopDto
            {
                SmallDistrictId = user.SmallDistrictId.ToString()
            }, cancelToken)).Select(x => new { ShopId = x.ShopId.ToString(), x.Shop.Name }).ToList();
            // var outDate = date.Select(x => new {  ShopId= x.ShopId.ToString(), x.Shop.Name }).ToList();
            return new ApiResult<dynamic>(APIResultCode.Success, date);
        }
    }
}
