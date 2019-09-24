using GuoGuoCommunity.API.Common;
using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Abstractions.Store;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderController : BaseController
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrdeItemRepository _ordeItemRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IShopUserAddressRepository _shopUserAddressRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly IWeiXinUserRepository _weiXinUserRepository;
        private readonly IActivityRepository _activityRepository;

        /// <summary>
        /// 
        /// </summary>
        public OrderController(
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IOrderRepository orderRepository,
            IShopUserAddressRepository shopUserAddressRepository,
            IOrdeItemRepository ordeItemRepository,
            IUserRepository userRepository,
            ISmallDistrictRepository smallDistrictRepository,
            ITokenRepository tokenRepository,
            IIndustryRepository industryRepository,
            IWeiXinUserRepository weiXinUserRepository,
            IActivityRepository activityRepository)
        {
            _tokenRepository = tokenRepository;
            _orderRepository = orderRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _shopUserAddressRepository = shopUserAddressRepository;
            _ordeItemRepository = ordeItemRepository;
            _userRepository = userRepository;
            _smallDistrictRepository = smallDistrictRepository;
            _industryRepository = industryRepository;
            _weiXinUserRepository = weiXinUserRepository;
            _activityRepository = activityRepository;
        }

        #region 业主小程序

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/add")]
        public async Task<ApiResult<AddOrderOutput>> Add([FromBody]AddOrderInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddOrderOutput>(APIResultCode.Unknown, new AddOrderOutput { }, APIResultMessage.TokenNull);
            }

            //if (string.IsNullOrWhiteSpace(input.ApplicationRecordId))
            //{
            //    throw new NotImplementedException("业主认证Id为空！");
            //}

            if (string.IsNullOrWhiteSpace(input.AddressId))
            {
                throw new NotImplementedException("用户地址Id为空！");
            }

            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                throw new NotImplementedException("商铺Id为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddOrderOutput>(APIResultCode.Unknown, new AddOrderOutput { }, APIResultMessage.TokenError);
            }
            //var ownerCertificationRecord = await _industryRepository.GetIncludeAsync(input.i, cancelToken);
            ////if (ownerCertificationRecord == null)
            ////{
            ////    throw new NotImplementedException("业主认证信息不正确！");
            ////}
            var shopUserAddress = await _shopUserAddressRepository.GetIncludeAsync(input.AddressId, cancelToken);
            if (shopUserAddress == null)
            {
                throw new NotImplementedException("收货地址信息不正确！");
            }
            if (shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.PropertyCompany == null)
            {
                throw new NotImplementedException("当前小区未配置服务物业不能进行下单！");
            }
            var entity = await _orderRepository.AddAsync(new OrderDto
            {
                ShopId = input.ShopId,
                DeliveryPhone = shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.PropertyCompany.Phone,
                DeliveryName = shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.PropertyCompany.Name,
                OrderStatusValue = OrderStatus.Unpaid.Value,
                OrderStatusName = OrderStatus.Unpaid.Name,
                //OwnerCertificationRecordId = input.ApplicationRecordId,
                ReceiverName = shopUserAddress.ReceiverName,
                ReceiverPhone = shopUserAddress.ReceiverPhone,
                Address = shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.State +
                shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.City +
                shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Region +
                shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Name +
                shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.Community.Name +
                shopUserAddress.Industry.BuildingUnit.Building.SmallDistrict.Name +
                shopUserAddress.Industry.BuildingUnit.Building.Name + 
                shopUserAddress.Industry.BuildingUnit.UnitName +
                shopUserAddress.Industry.NumberOfLayers + "," +
                shopUserAddress.Industry.Name,
                IndustryId = shopUserAddress.IndustryId.ToString(),
                Number = GenerateCode(""),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            //try
            //{
            //    var shopUserList = await _userRepository.GetByShopIdAsync(entity.ShopId.ToString(), cancelToken);
            //    foreach (var item in shopUserList)
            //    {
            //        SignalR("2", entity.ShopId.ToString(), item.Id.ToString(), entity);
            //    }

            //}
            //catch (Exception)
            //{


            //}

            return new ApiResult<AddOrderOutput>(APIResultCode.Success, new AddOrderOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [Obsolete]
        public ApiResult Test(CancellationToken cancelToken)
        {
            var conid = SignalRServerHub.ConnectionIds.FirstOrDefault(a => a.Key == "2@" + "04e66fda-4680-e911-b788-b42e99198025" + "@" + "128ddda4-4780-e911-b788-b42e99198025").Value;
            if (!string.IsNullOrEmpty(conid))
            {
                SignalRServerHub.ClientList.Client(conid).getorderinfo("d884981d-1f81-e911-b789-b42e99198025");
            }
            else
            {

            }


            return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 
        /// </summary>
        public class Model
        {
            /// <summary>
            /// 
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string CreateOperationTime { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int ShopCommodityCount { get; set; }
        }

        private void SignalR(string type, string companyID, string employeeId, Order order)
        {
            var conid = SignalRServerHub.ConnectionIds.FirstOrDefault(a => a.Key == type + "@" + companyID + "@" + employeeId).Value;
            if (!string.IsNullOrEmpty(conid))
            {
                //    foreach (var item in conid)
                //    {
                SignalRServerHub.ClientList.Client(conid).getorderinfo(new Model
                {
                    Id = order.Id.ToString(),
                    CreateOperationTime = order.CreateOperationTime.Value.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
                    ShopCommodityCount = order.ShopCommodityCount
                });//order.Id.ToString() + "@" + order.CreateOperationTime + "@" + order.ShopCommodityCount) ;
            }

            //}
            //if (!string.IsNullOrEmpty(conid))
            //{

            //}
            //else
            //{

            //}


            // return new ApiResult(APIResultCode.Success);
        }

        /// <summary>
        /// 分页查询订单(小程序)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/getAllForPage")]
        public async Task<ApiResult<GetAllForPageOutput>> GetListForPage([FromUri]GetAllForPageInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllForPageOutput>(APIResultCode.Success_NoB, new GetAllForPageOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllForPageOutput>(APIResultCode.Unknown, new GetAllForPageOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForPageOutput>(APIResultCode.Unknown, new GetAllForPageOutput { }, APIResultMessage.TokenError);
            }
            //if (string.IsNullOrWhiteSpace(input.ApplicationRecordId))
            //{
            //    return new ApiResult<GetAllForPageOutput>(APIResultCode.Success_NoB, new GetAllForPageOutput { }, "业主认证Id为空");
            //}
            var date = await _orderRepository.GetAllIncludeForPageAsync(new OrderDto
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                OrderStatusValue = input.OrderStatusValue,
                OperationUserId = user.Id.ToString()
                //OwnerCertificationRecordId = input.ApplicationRecordId
            }, cancelToken);
            var ordeItemList = await _ordeItemRepository.GetListIncludeForOrderIdsAsync(date.List.Select(x => x.Id.ToString()).ToList(), cancelToken);

            List<GetAllForPageOutputModel> list = new List<GetAllForPageOutputModel>();
            foreach (var item in date.List)
            {
                var itemList = new List<OrdeItemModel>();
                foreach (var ordeItem in ordeItemList.Where(x => x.OrderId == item.Id))
                {
                    itemList.Add(new OrdeItemModel
                    {
                        CommodityCount = ordeItem.CommodityCount,
                        DiscountPrice = ordeItem.DiscountPrice,
                        ImageUrl = ordeItem.ImageUrl,
                        Name = ordeItem.Name,
                        Price = ordeItem.Price
                    });
                }
                list.Add(new GetAllForPageOutputModel
                {
                    Id = item.Id.ToString(),
                    DeliveryName = item.DeliveryName,
                    DeliveryPhone = item.DeliveryPhone,
                    Number = item.Number,
                    OrderStatusName = item.OrderStatusName,
                    OrderStatusValue = item.OrderStatusValue,
                    PaymentPrice = item.PaymentPrice,
                    Postage = item.Postage,
                    ShopCommodityCount = item.ShopCommodityCount,
                    ShopCommodityPrice = item.ShopCommodityPrice,
                    ShopId = item.ShopId.ToString(),
                    ShopName = item.Shop.Name,
                    LogoUrl = item.Shop.LogoImageUrl,
                    List = itemList,
                    SmallDistrictShopId = item.SmallDistrictShopId.ToString(),
                    PaymentStatusName = item.PaymentStatusName,
                    PaymentStatusValue = item.PaymentStatusValue
                });
            }
            return new ApiResult<GetAllForPageOutput>(APIResultCode.Success, new GetAllForPageOutput
            {
                List = list,
                TotalCount = date.Count
            });
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/get")]
        public async Task<ApiResult<GetOrderOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetOrderOutput>(APIResultCode.Unknown, new GetOrderOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetOrderOutput>(APIResultCode.Unknown, new GetOrderOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("订单Id信息为空！");
            }

            var data = await _orderRepository.GetIncludeAsync(id, cancelToken);

            var list = await _ordeItemRepository.GetListIncludeForOrderIdAsync(id, cancelToken);

            var itemList = new List<OrdeItemModel>();

            foreach (var ordeItem in list)
            {
                itemList.Add(new OrdeItemModel
                {
                    OriginalPrice = ordeItem.Price,
                    CommodityCount = ordeItem.CommodityCount,
                    DiscountPrice = ordeItem.DiscountPrice,
                    ImageUrl = ordeItem.ImageUrl,
                    Name = ordeItem.Name,
                    Price = ordeItem.Price
                });
            }
            List<Activity> alist = new List<Activity>();
            int activitySource = 1;
            if (data.Shop != null && !string.IsNullOrEmpty(data.Shop.ActivitySign) && data.Shop.ActivitySign != "0")
            {
                alist = (await _activityRepository.GetAllAsync(new Domain.Dto.Store.ActivityDto
                {
                    IsSelectByTime = true,
                    ActivitySource = 2

                }, cancelToken)).Select(x => new Activity
                {
                    ActivitySource = x.ActivitySource,
                    ActivityType = x.ActivityType,
                    ID = x.ID.ToString(),
                    Money = x.Money,
                    Off = x.Off,
                    ActivityBeginTime = x.ActivityBeginTime,
                    ActivityEndTime = x.ActivityEndTime,
                    ShopId = x.ShopId.ToString()
                }).OrderBy(b => b.Money).ToList();
                activitySource = 2;
            }
            alist = (await _activityRepository.GetAllAsync(new Domain.Dto.Store.ActivityDto
            {
                ShopId = data.ShopId.ToString(),
                IsSelectByTime = true,
                ActivitySource = 1
            }, cancelToken)).Select(x => new Activity
            {
                ActivitySource = x.ActivitySource,
                ActivityType = x.ActivityType,
                ID = x.ID.ToString(),
                Money = x.Money,
                Off = x.Off,
                ActivityBeginTime = x.ActivityBeginTime,
                ActivityEndTime = x.ActivityEndTime,
                ShopId = x.ShopId.ToString()
            }).OrderByDescending(b => b.Money).ToList();

            //判断如果没有店铺活动则查询平台活动
            if (alist == null || alist.Count == 0)
            {
                alist = (await _activityRepository.GetAllAsync(new Domain.Dto.Store.ActivityDto
                {
                    IsSelectByTime = true,
                    ActivitySource = 2

                }, cancelToken)).Select(x => new Activity
                {
                    ActivitySource = x.ActivitySource,
                    ActivityType = x.ActivityType,
                    ID = x.ID.ToString(),
                    Money = x.Money,
                    Off = x.Off,
                    ActivityBeginTime = x.ActivityBeginTime,
                    ActivityEndTime = x.ActivityEndTime,
                    ShopId = x.ShopId.ToString()
                }).OrderByDescending(b => b.Money).ToList();
                activitySource = 2;
            }
            decimal shopCommodityRealPrice = data.ShopCommodityPrice;
            decimal paymentRealPrice = data.PaymentPrice;
            decimal off = 0;
            foreach (Activity item in alist)
            {
                if (item.Money <= data.PaymentPrice)
                {
                    shopCommodityRealPrice = data.ShopCommodityPrice - item.Off;
                    paymentRealPrice = data.PaymentPrice - item.Off;
                    off = item.Off;
                    break;
                }
            }

            return new ApiResult<GetOrderOutput>(APIResultCode.Success, new GetOrderOutput
            {
                Id = data.Id.ToString(),
                DeliveryName = data.DeliveryName,
                DeliveryPhone = data.DeliveryPhone,
                Number = data.Number,
                OrderStatusName = data.OrderStatusName,
                OrderStatusValue = data.OrderStatusValue,
                PaymentPrice = paymentRealPrice,
                Postage = data.Postage,
                ShopCommodityCount = data.ShopCommodityCount,
                ShopCommodityPrice = shopCommodityRealPrice,
                ShopId = data.ShopId.ToString(),
                ShopName = data.Shop.Name,
                LogoUrl = data.Shop.LogoImageUrl,
                List = itemList,
                CreateTime = data.CreateOperationTime.Value,
                Address = data.Address,
                ReceiverName = data.ReceiverName,
                ReceiverPhone = data.ReceiverPhone,
                PaymentStatusValue=data.PaymentStatusValue,
                PaymentStatusName=data.PaymentStatusName,
                Off = off
            });
        }

        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/delete")]
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

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _orderRepository.DeleteAsync(new OrderDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/onFinish")]
        public async Task<ApiResult> Update([FromBody]UpdateOrderInput input, CancellationToken cancellationToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var data = await _orderRepository.GetIncludeAsync(input.Id, cancellationToken);
            if (data.OrderStatusValue != OrderStatus.WaitingReceive.Value)
            {
                throw new NotImplementedException("不是有效状态值！");
            }

            await _orderRepository.UpdateAsync(new OrderDto
            {
                Id = input.Id,
                OrderStatusValue = OrderStatus.Finish.Value,
                OrderStatusName = OrderStatus.Finish.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });

            return new ApiResult();
        }

        #endregion

        #region 物业端

        /// <summary>
        /// 分页查询订单(物业端)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/getAllForProperty")]
        public async Task<ApiResult<GetAllOutput>> GetListForProperty([FromUri]GetAllOrderForPropertyInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Success_NoB, new GetAllOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Unknown, new GetAllOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Unknown, new GetAllOutput { }, APIResultMessage.TokenError);
            }

            if (user.DepartmentValue != Department.WuYe.Value)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Success_NoB, new GetAllOutput { }, "操作人部门不为物业");
            }
            var date = await _orderRepository.GetAllIncludeForPropertyAsync(new OrderDto
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                OrderStatusValue = input.OrderStatusValue,
                ShopId = input.ShopId,
                SmallDistrictId = user.SmallDistrictId,
                Number = input.Number
            }, cancelToken);

            List<GetAllOutputModel> list = new List<GetAllOutputModel>();
            foreach (var item in date.List)
            {

                list.Add(new GetAllOutputModel
                {
                    Id = item.Id.ToString(),
                    DeliveryName = item.DeliveryName,
                    DeliveryPhone = item.DeliveryPhone,
                    Number = item.Number,
                    OrderStatusName = item.OrderStatusName,
                    OrderStatusValue = item.OrderStatusValue,
                    PaymentPrice = item.PaymentPrice,
                    Postage = item.Postage,
                    ShopCommodityCount = item.ShopCommodityCount,
                    ShopCommodityPrice = item.ShopCommodityPrice,
                    ShopId = item.ShopId.ToString(),
                    ShopName = item.Shop.Name,
                    LogoUrl = item.Shop.LogoImageUrl,
                    IsBtnDisplay = item.OrderStatusValue == OrderStatus.WaitingSend.Value,
                    CreateTime = item.CreateOperationTime.Value,
                    // List = itemList
                });
            }
            return new ApiResult<GetAllOutput>(APIResultCode.Success, new GetAllOutput { List = list, TotalCount = date.Count });
        }

        /// <summary>
        /// 安排取货
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/onSend")]
        public async Task<ApiResult> UpdateSend([FromBody]UpdateOrderInput input, CancellationToken cancellationToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var data = await _orderRepository.GetIncludeAsync(input.Id, cancellationToken);
            if (data.OrderStatusValue != OrderStatus.WaitingSend.Value)
            {
                throw new NotImplementedException("不是有效状态值！");
            }

            await _orderRepository.UpdateAsync(new OrderDto
            {
                Id = input.Id,
                OrderStatusValue = OrderStatus.WaitingTake.Value,
                OrderStatusName = OrderStatus.WaitingTake.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });

            var shopUserList = await _userRepository.GetByShopIdAsync(data.ShopId.ToString(), cancellationToken);
            foreach (var item in shopUserList)
            {
                SignalR("2", data.ShopId.ToString(), item.Id.ToString(), data);
            }

            //await OrderPushRemind(new OrderPushModel
            //{
            //    Type = "已配送",
            //    CreateTime = data.CreateOperationTime.Value,
            //    Id = data.Id.ToString(),
            //    Number = data.Number,
            //    PaymentPrice = data.PaymentPrice,
            //    ReceiverName = data.ReceiverName
            //}, data.OwnerCertificationRecordId.ToString());

            return new ApiResult();
        }

        #endregion

        #region 商家端

        /// <summary>
        /// 分页查询订单(商家端)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/getAllForMerchant")]
        public async Task<ApiResult<GetAllOutput>> GetListForMerchant([FromUri]GetAllForMerchantInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Success_NoB, new GetAllOutput { }, "参数无效");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Unknown, new GetAllOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Unknown, new GetAllOutput { }, APIResultMessage.TokenError);
            }

            if (user.DepartmentValue != Department.Shop.Value)
            {
                return new ApiResult<GetAllOutput>(APIResultCode.Success_NoB, new GetAllOutput { }, "操作人部门不为商户");
            }

            var date = await _orderRepository.GetAllIncludeForMerchantAsync(new OrderDto
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                OrderStatusValue = input.OrderStatusValue,
                Number = input.Number,
                ShopId = user.ShopId.ToString()
            }, cancelToken);

            List<GetAllOutputModel> list = new List<GetAllOutputModel>();
            foreach (var item in date.List)
            {

                list.Add(new GetAllOutputModel
                {
                    Id = item.Id.ToString(),
                    DeliveryName = item.DeliveryName,
                    DeliveryPhone = item.DeliveryPhone,
                    Number = item.Number,
                    OrderStatusName = item.OrderStatusName,
                    OrderStatusValue = item.OrderStatusValue,
                    PaymentPrice = item.PaymentPrice,
                    Postage = item.Postage,
                    ShopCommodityCount = item.ShopCommodityCount,
                    ShopCommodityPrice = item.ShopCommodityPrice,
                    ShopId = item.ShopId.ToString(),
                    ShopName = item.Shop.Name,
                    LogoUrl = item.Shop.LogoImageUrl,
                    IsBtnDisplay = item.OrderStatusValue == OrderStatus.WaitingTake.Value,
                    IsAcceptBtnDisplay = item.OrderStatusValue == OrderStatus.WaitingAccept.Value,
                    CreateTime = item.CreateOperationTime.Value,
                });
            }
            return new ApiResult<GetAllOutput>(APIResultCode.Success, new GetAllOutput { List = list, TotalCount = date.Count });
        }

        /// <summary>
        /// 发送货品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/onReceive")]
        public async Task<ApiResult> UpdateReceive([FromBody]UpdateOrderInput input, CancellationToken cancellationToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var data = await _orderRepository.GetIncludeAsync(input.Id, cancellationToken);
            if (data.OrderStatusValue != OrderStatus.WaitingTake.Value)
            {
                throw new NotImplementedException("不是有效状态值！");
            }

            await _orderRepository.UpdateAsync(new OrderDto
            {
                Id = input.Id,
                OrderStatusValue = OrderStatus.WaitingReceive.Value,
                OrderStatusName = OrderStatus.WaitingReceive.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),

            });

            return new ApiResult();
        }

        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/onAccept")]
        public async Task<ApiResult> UpdateAccept([FromBody]UpdateOrderInput input, CancellationToken cancellationToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var data = await _orderRepository.GetIncludeAsync(input.Id, cancellationToken);
            if (data.OrderStatusValue != OrderStatus.WaitingAccept.Value)
            {
                throw new NotImplementedException("不是有效状态值！");
            }

            await _orderRepository.UpdateAsync(new OrderDto
            {
                Id = input.Id,
                OrderStatusValue = OrderStatus.WaitingSend.Value,
                OrderStatusName = OrderStatus.WaitingSend.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),

            });

            var propertyUserList = await _userRepository.GetAllPropertyAsync(new UserDto { SmallDistrictId = data.Industry.BuildingUnit.Building.SmallDistrictId.ToString() }, cancellationToken);

            foreach (var item in propertyUserList)
            {
                var smallDistrict = await _smallDistrictRepository.GetAsync(item.SmallDistrictId, cancellationToken);
                SignalR("1", smallDistrict.PropertyCompanyId.ToString(), item.Id.ToString(), data);
            }

            await OrderPushRemind(new OrderPushModel
            {
                Type = "已接单",
                CreateTime = data.CreateOperationTime.Value,
                Id = data.Id.ToString(),
                Number = data.Number,
                PaymentPrice = data.PaymentPrice,
                ReceiverName = data.ReceiverName
            }, data.CreateOperationUserId.ToString());

            return new ApiResult();
        }

        #endregion

        /// <summary>
        /// 发送推送消息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public async Task OrderPushRemind(OrderPushModel model, string userId)
        {
            var userEntity = await _userRepository.GetForIdAsync(userId);
            var weiXinUser = await _weiXinUserRepository.GetAsync(userEntity.UnionId);

            try
            {
                var templateData = new
                {
                    first = new TemplateDataItem("您好，您的订单" + model.Type),
                    keyword1 = new TemplateDataItem(model.Number),
                    keyword2 = new TemplateDataItem(model.CreateTime.ToString("yyyy年MM月dd日 HH:mm:ss")),
                    keyword3 = new TemplateDataItem(model.ReceiverName),
                    keyword4 = new TemplateDataItem(model.PaymentPrice.ToString()),
                    remark = new TemplateDataItem(">>点击查看详情<<", "#FF0000")
                };
                var miniProgram = new TempleteModel_MiniProgram()
                {
                    appid = GuoGuoCommunity_WxOpenAppId,
                    pagepath = "pages/orderDetail/orderDetail?id=" + model.Id
                };
                TemplateApi.SendTemplateMessage(AppId, weiXinUser?.OpenId, OrderTemplateId, null, templateData, miniProgram);
            }
            catch (Exception)
            {

            }
        }
    }
}
