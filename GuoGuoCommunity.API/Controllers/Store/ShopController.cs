using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.API.Models.Shop;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Abstractions.Store;
using GuoGuoCommunity.Domain.Dto;
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
    ///  商户信息
    /// </summary>
    public class ShopController : BaseController
    {
        private readonly IShopRepository _shopRepository;
        private readonly ISmallDistrictShopRepository _smallDistrictShopRepository;
        private readonly IShoppingTrolleyRepository _shoppingTrolleyRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IActivityRepository _activityRepository;

        /// <summary>
        /// 商户信息
        /// </summary>
        public ShopController(
            IShopRepository shopRepository,
            IShoppingTrolleyRepository shoppingTrolleyRepository,
            ISmallDistrictShopRepository smallDistrictShopRepository,
            IActivityRepository activityRepository,
            ITokenRepository tokenRepository)
        {
            _shopRepository = shopRepository;
            _shoppingTrolleyRepository = shoppingTrolleyRepository;
            _smallDistrictShopRepository = smallDistrictShopRepository;
            _tokenRepository = tokenRepository;
            _activityRepository = activityRepository;
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shop/add")]
        public async Task<ApiResult<AddShopOutput>> GetAll([FromBody]AddShopInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Unknown, new AddShopOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Unknown, new AddShopOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Success_NoB, new AddShopOutput { }, "商户名称为空！");
            }
            if (string.IsNullOrWhiteSpace(input.PhoneNumber))
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Success_NoB, new AddShopOutput { }, "商户手机号为空！");
            }

            if (string.IsNullOrWhiteSpace(input.Address))
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Success_NoB, new AddShopOutput { }, "地址为空！");
            }
            if (string.IsNullOrWhiteSpace(input.MerchantCategoryValue))
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Success_NoB, new AddShopOutput { }, "商超类别为空！");
            }
            if (string.IsNullOrWhiteSpace(input.QualificationImageUrl))
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Success_NoB, new AddShopOutput { }, "资质图片为空！");
            }


            var merchantCategory = MerchantCategory.GetAll().Where(x => x.Value == input.MerchantCategoryValue).FirstOrDefault();
            if (merchantCategory == null)
            {
                return new ApiResult<AddShopOutput>(APIResultCode.Success_NoB, new AddShopOutput { }, "商超类型不正确！");
            }

            // var qualificationImage = await _uploadRepository.GetAsync(input.QualificationImageId, cancelToken);
            ShopDto dto = new ShopDto
            {
                Address = input.Address,
                Description = input.Description,
                MerchantCategoryValue = merchantCategory.Value,
                MerchantCategoryName = merchantCategory.Name,
                Name = input.Name,
                QualificationImageUrl = input.QualificationImageUrl,//qualificationImage.Agreement + qualificationImage.Host + qualificationImage.Domain + qualificationImage.Directory + qualificationImage.File,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                PhoneNumber = input.PhoneNumber,
                PrinterName = input.PrinterName,
                LogoImageUrl = input.QualificationImageUrl
            };
            //if (!string.IsNullOrWhiteSpace(input.QualificationImageUrl))
            //{
            //    var logoImage = await _uploadRepository.GetAsync(input.LogoImageId, cancelToken);
            dto.LogoImageUrl = input.LogoImageUrl;
            //}
            var entity = await _shopRepository.AddAsync(dto, cancelToken);

            return new ApiResult<AddShopOutput>(APIResultCode.Success, new AddShopOutput { Id = entity.Id.ToString() }, APIResultMessage.Success);
        }

        /// <summary>
        /// 查询商户列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shop/getAll")]
        public async Task<ApiResult<GetAllShopOutput>> GetAll([FromUri]GetAllShopInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllShopOutput>(APIResultCode.Unknown, new GetAllShopOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllShopOutput>(APIResultCode.Unknown, new GetAllShopOutput { }, APIResultMessage.TokenError);
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

            var data = await _shopRepository.GetAllIncludeAsync(new ShopDto
            {
                PhoneNumber = input.PhoneNumber,
                Name = input.Name,
                MerchantCategoryValue = input.MerchantCategoryValue
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllShopOutput>(APIResultCode.Success, new GetAllShopOutput
            {
                List = list.Select(x => new GetAllShopOutputModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    MerchantCategoryValue = x.MerchantCategoryValue,
                    PhoneNumber = x.PhoneNumber,
                    CreateTime = x.CreateOperationTime.Value,
                    MerchantCategoryName = x.MerchantCategoryName,
                    Address = x.Address
                }).ToList(),
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 删除商户账户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shop/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("商户Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _shopRepository.DeleteAsync(new ShopDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 获取商户详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shop/get")]
        public async Task<ApiResult<GetShopOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetShopOutput>(APIResultCode.Unknown, new GetShopOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetShopOutput>(APIResultCode.Unknown, new GetShopOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("商户Id信息为空！");
            }
            var data = await _shopRepository.GetIncludeAsync(id, cancelToken);

            return new ApiResult<GetShopOutput>(APIResultCode.Success, new GetShopOutput
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                Address = data.Address,
                CreateTime = data.CreateOperationTime.Value,
                Description = data.Description,
                LogoImageUrl = data.LogoImageUrl,
                MerchantCategoryName = data.MerchantCategoryName,
                MerchantCategoryValue = data.MerchantCategoryValue,
                PhoneNumber = data.PhoneNumber,
                QualificationImageUrl = data.QualificationImageUrl,
                PrinterName = data.PrinterName
            });
        }

        /// <summary>
        /// 修改商户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shop/update")]
        public async Task<ApiResult> Update([FromBody]UpdateShopInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.PhoneNumber))
            {
                return new ApiResult(APIResultCode.Success_NoB, "商户手机号为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Address))
            {
                return new ApiResult(APIResultCode.Success_NoB, "地址为空！");
            }
            if (string.IsNullOrWhiteSpace(input.MerchantCategoryValue))
            {
                return new ApiResult(APIResultCode.Success_NoB, "商户类别为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                return new ApiResult(APIResultCode.Success_NoB, "商户名称为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            var merchantCategory = MerchantCategory.GetAll().Where(x => x.Value == input.MerchantCategoryValue).FirstOrDefault();
            if (merchantCategory == null)
            {
                return new ApiResult(APIResultCode.Success_NoB, "商超类型不正确！");
            }
            ShopDto dto = new ShopDto
            {
                Id = input.Id,
                Name = input.Name,
                Address = input.Address,
                Description = input.Description,
                MerchantCategoryValue = merchantCategory.Value,
                MerchantCategoryName = merchantCategory.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                PhoneNumber = input.PhoneNumber,
                PrinterName = input.PrinterName,
                LogoImageUrl = input.LogoImageUrl,
                QualificationImageUrl = input.QualificationImageUrl
            };

            if (await _shopRepository.UpdateAsync(dto, cancelToken) > 0)
            {
                return new ApiResult(APIResultCode.Success, APIResultMessage.Success);
            }
            return new ApiResult(APIResultCode.Success_NoB, "数据更新失败！");
        }

        /// <summary>
        /// 获取所有商户集合
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shop/getList")]
        public async Task<ApiResult<List<GetListShopOutput>>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListShopOutput>>(APIResultCode.Unknown, new List<GetListShopOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListShopOutput>>(APIResultCode.Unknown, new List<GetListShopOutput> { }, APIResultMessage.TokenError);
            }
            var data = (await _shopRepository.GetListAsync(new ShopDto
            {

            }, cancelToken)).Select(x => new GetListShopOutput
            {
                Id = x.Id.ToString(),
                Name = x.Name
            }).ToList();
            return new ApiResult<List<GetListShopOutput>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 小程序获取商户详情
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("shop/getForShopUser")]
        public async Task<ApiResult<GetForShopUserOutput>> GetForShopUser([FromUri]GetForShopUserInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetForShopUserOutput>(APIResultCode.Unknown, new GetForShopUserOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetForShopUserOutput>(APIResultCode.Unknown, new GetForShopUserOutput { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrWhiteSpace(input.ShopId))
            {
                throw new NotImplementedException("商户Id信息为空！");
            }
            var data = await _smallDistrictShopRepository.GetIncludeAsync(input.ShopId, cancelToken);

            var shoppingTrolleyList = await _shoppingTrolleyRepository.GetAllIncludeAsync(new ShoppingTrolleyDto
            {
                //OwnerCertificationRecordId = input.ApplicationRecordId,
                OperationUserId = user.Id.ToString(),
                ShopId = input.ShopId
            }, cancelToken);
            List<Activity> alist = new List<Activity>();
            int activitySource = 1;
            if (data.Shop.ActivitySign == "0" || string.IsNullOrEmpty(data.Shop.ActivitySign))
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
            else
            {

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
                }).OrderBy(b => b.Money).ToList();
                
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
                    }).OrderBy(b => b.Money).ToList();
                    activitySource = 2;
                }
            }
            

            
            return new ApiResult<GetForShopUserOutput>(APIResultCode.Success, new GetForShopUserOutput
            {
                Id = data.Id.ToString(),
                Name = data.Shop.Name,
                Address = data.Shop.Address,
                LogoImageUrl = data.Shop.LogoImageUrl,
                PhoneNumber = data.Shop.PhoneNumber,
                IsPresence = shoppingTrolleyList.Any(),
                Postage = data.Postage,
                ShopActivityList = alist,
                ActivitySource = activitySource
            });
        }


        /// <summary>
        /// 更新店铺开启的活动
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shop/updateActivitySign")]
        public async Task<ApiResult> UpdateShopActivitySign([FromBody]UpdateShopSignInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                return new ApiResult(APIResultCode.Success_NoB, "店铺ID无效！");
            }
            if (string.IsNullOrWhiteSpace(input.ActivitySign))
            {
                return new ApiResult(APIResultCode.Success_NoB, "活动标记无效！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            ShopDto dto = new ShopDto
            {
                Id = input.Id,
                ActivitySign = input.ActivitySign,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            };

            if (await _shopRepository.UpdateShopActivitySign(dto, cancelToken))
            {
                return new ApiResult(APIResultCode.Success, APIResultMessage.Success);
            }
            return new ApiResult(APIResultCode.Success_NoB, "数据更新失败！");
        }
    }
}
