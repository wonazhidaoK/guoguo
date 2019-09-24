using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Abstractions.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers.Store
{
    public class ActivityController : BaseController
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository, ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            _activityRepository = activityRepository;
        }
        /// <summary>
        /// 创建活动信息
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("activity/add")]
        public async Task<ApiResult<Activity>> Add([FromBody] Activity activity, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<Activity>(APIResultCode.Unknown, new Activity { }, APIResultMessage.TokenNull);
            }
            if (activity.Money <= 0)
            {
                throw new NotImplementedException("活动触发金额必须大于0！");
            }
            if (activity.Off <= 0)
            {
                throw new NotImplementedException("活动减免金额必须大于0");
            }
            if (activity.ActivityBeginTime >= activity.ActivityEndTime)
            {
                throw new NotImplementedException("活动开始时间必须大于结束时间");
            }
            if (activity.ActivitySource <= 0 || activity.ActivitySource > 2)
            {
                throw new NotImplementedException("活动来源无效");
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<Activity>(APIResultCode.Unknown, new Activity { }, APIResultMessage.TokenError);
            }
            var entity = await _activityRepository.AddAsync(new Domain.Dto.Store.ActivityDto
            {
                ActivitySource = activity.ActivitySource,
                ActivityType = activity.ActivityType,
                Money = activity.Money,
                Off = activity.Off,
                ShopId = activity.ShopId,
                ActivityBeginTime = activity.ActivityBeginTime,
                ActivityEndTime = activity.ActivityEndTime,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            },cancelToken);

            return new ApiResult<Activity>(APIResultCode.Success, new Activity { ID = entity.ID.ToString() });

        }
        /// <summary>
        /// 删除活动信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("activity/delete")]
        public async Task<ApiResult> Delete([FromUri] string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("活动Id信息为空！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }

            await _activityRepository.DeleteAsync(new Domain.Dto.Store.ActivityDto
            {
                ID = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            },cancelToken);

            return new ApiResult();
        }

        /// <summary>
        /// 修改活动分类
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("activity/update")]
        public async Task<ApiResult> Update([FromBody]Activity activity, CancellationToken cancelToken)
        {

            //if (Authorization == null)
            //{
            //    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            //}

            if (activity.Money <= 0)
            {
                throw new NotImplementedException("活动触发金额必须大于0！");
            }
            if (activity.Off <= 0)
            {
                throw new NotImplementedException("活动减免金额必须大于0");
            }
            //var user = _tokenRepository.GetUser(Authorization);
            //if (user == null)
            //{
            //    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            //}

            await _activityRepository.UpdateAsync(new Domain.Dto.Store.ActivityDto
            {
                ID = activity.ID,
                Money = activity.Money,
                Off = activity.Off,
                ShopId = activity.ShopId,
                ActivityType = activity.ActivityType,
                ActivitySource = activity.ActivitySource,
                ActivityBeginTime = activity.ActivityBeginTime,
                ActivityEndTime = activity.ActivityEndTime,
                OperationTime = DateTimeOffset.Now
                //OperationUserId = user.Id.ToString()
            },cancelToken);
            return new ApiResult();
        }


        /// <summary>
        /// 分页查询所有商品分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("activity/getAllForShop")]
        public async Task<ApiResult<List<Activity>>> GetListForShop([FromUri]string shopId,CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<Activity>>(APIResultCode.Unknown, new List<Activity> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<Activity>>(APIResultCode.Unknown, new List<Activity> { }, APIResultMessage.TokenError);
            }
            if (string.IsNullOrEmpty(shopId))
            {
                throw new NotImplementedException("商户Id为空！");
            }
            var data = (await _activityRepository.GetAllAsync(new Domain.Dto.Store.ActivityDto
            {
                ShopId = shopId,
                ActivitySource = 1
            },cancelToken)).Select(x=> new Activity {
                ActivitySource = x.ActivitySource,
                ActivityType = x.ActivityType,
                ID = x.ID.ToString(),
                Money = x.Money,
                Off = x.Off,
                ActivityBeginTime = x.ActivityBeginTime,
                ActivityEndTime = x.ActivityEndTime,
                ShopId = x.ShopId.ToString()
            }).ToList();
            return new ApiResult<List<Activity>>(APIResultCode.Success, data);
        }

        /// <summary>
        /// 分页查询所有商品分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("activity/getAllForPlatform")]
        public async Task<ApiResult<List<Activity>>> GetListForPlatform(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<Activity>>(APIResultCode.Unknown, new List<Activity> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<Activity>>(APIResultCode.Unknown, new List<Activity> { }, APIResultMessage.TokenError);
            }
            var data = (await _activityRepository.GetAllAsync(new Domain.Dto.Store.ActivityDto
            {
                ActivitySource = 2
            }, cancelToken)).Select(x => new Activity
            {
                ActivitySource = x.ActivitySource,
                ActivityType = x.ActivityType,
                ID = x.ID.ToString(),
                Money = x.Money,
                Off = x.Off,
                ActivityBeginTime = x.ActivityBeginTime,
                ActivityEndTime = x.ActivityEndTime
            }).ToList();
            return new ApiResult<List<Activity>>(APIResultCode.Success, data);
        }
    }
}
