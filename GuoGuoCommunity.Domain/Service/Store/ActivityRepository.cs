using GuoGuoCommunity.Domain.Abstractions.Store;
using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service.Store
{
    public class ActivityRepository : IActivityRepository
    {
        public async Task<Activity> AddAsync(ActivityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                
                //判断如果创建的是平台活动
                if (dto.ActivitySource == 2)
                {
                    var activity = await db.Activities.Where(x => x.Money == dto.Money && x.ActivitySource == dto.ActivitySource && x.IsDeleted == false).FirstOrDefaultAsync(token);
                    if (activity != null)
                    {
                        throw new NotImplementedException("该档满减活动已存在！");
                    }

                    var entity = db.Activities.Add(new Activity
                    {
                        ActivitySource = dto.ActivitySource,
                        Money = dto.Money,
                        ActivityType = dto.ActivityType,
                        Off = dto.Off,
                        ActivityBeginTime = dto.ActivityBeginTime,
                        ActivityEndTime = dto.ActivityEndTime
                    });
                    await db.SaveChangesAsync(token);

                    return entity;
                }
                //判断如果创建的是店铺活动
                else
                {
                    if (!Guid.TryParse(dto.ShopId, out var shopId))
                    {
                        throw new NotImplementedException("商店id信息不正确！");
                    }

                    var shop = await db.Shops.Where(x => x.Id == shopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                    if (shop == null)
                    {
                        throw new NotImplementedException("该商家不存在！");
                    }
                    var activity = await db.Activities.Where(x => x.Money == dto.Money && x.ShopId.ToString() == dto.ShopId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                    if (activity != null)
                    {
                        throw new NotImplementedException("该档满减活动已存在！");
                    }

                    var entity = db.Activities.Add(new Activity
                    {
                        ActivitySource = dto.ActivitySource,
                        Money = dto.Money,
                        ShopId = shopId,
                        ActivityType = dto.ActivityType,
                        Off = dto.Off,
                        ActivityBeginTime = dto.ActivityBeginTime,
                        ActivityEndTime = dto.ActivityEndTime
                    });
                    await db.SaveChangesAsync(token);

                    return entity;
                }
            }
        }

        public async Task DeleteAsync(ActivityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ID, out var uid))
                {
                    throw new NotImplementedException("活动ID信息不正确！");
                }
                var activity = await db.Activities.Where(x => x.ID == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (activity == null)
                {
                    throw new NotImplementedException("该活动信息不存在，或以被删除！");
                }
                activity.LastOperationTime = dto.OperationTime;
                activity.LastOperationUserId = dto.OperationUserId;
                activity.DeletedTime = dto.OperationTime;
                activity.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Activity>> GetAllAsync(ActivityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var activityList = db.Activities.Where(x => x.IsDeleted == false);
                if (dto.ActivitySource == 1)//如果是查询商家活动列表
                {
                    if (!Guid.TryParse(dto.ShopId, out var shopId))
                    {
                        throw new NotImplementedException("商店id信息不正确！");
                    }
                    activityList = activityList.Where(item => item.ShopId == shopId);
                }
                else
                {
                    activityList = activityList.Where(item => item.ActivitySource == dto.ActivitySource);
                }
                if (dto.IsSelectByTime)
                {
                    activityList = activityList.Where(x => x.ActivityBeginTime <= DateTime.Now && x.ActivityEndTime > DateTime.Now);
                }                
                List<Activity> resultList = await activityList.OrderBy(x => x.Money).ToListAsync(token);
                return resultList;
            }
        }

        public Task<List<Activity>> GetAllIncludeAsync(ActivityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Activity> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                Activity activity = await db.Activities.Where(x => x.ID.ToString() == id).FirstOrDefaultAsync(token);
                return activity;
            }
        }

        public Task<Activity> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Activity>> GetListAsync(ActivityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Activity>> GetListIncludeAsync(ActivityDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ActivityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ID, out var ID))
                {
                    throw new NotImplementedException("活动ID无效！");
                }
                var activity = await db.Activities.Where(item => item.ID == ID).FirstOrDefaultAsync(token);
                if (activity == null)
                {
                    throw new NotImplementedException("活动信息不存在，或已经被删除！");
                }

                var model = db.Activities.Where(x => x.Money == dto.Money && x.IsDeleted == false && x.ID != ID);
                if (dto.ActivitySource == 1)
                {
                    model = model.Where(x => x.ShopId.ToString() == dto.ShopId && x.ActivitySource == 1);
                }
                else
                    model = model.Where(x => x.ActivitySource == 2);
                Activity activityresult = await model.FirstOrDefaultAsync(token);
                if (activityresult != null && !string.IsNullOrEmpty(activityresult.ID.ToString()))
                {
                    throw new NotImplementedException("已经有此档活动存在，请勿重复设置！");
                }
                activity.Money = dto.Money;
                activity.Off = dto.Off;
                activity.ActivityBeginTime = dto.ActivityBeginTime;
                activity.ActivityEndTime = dto.ActivityEndTime;
                activity.LastOperationTime = dto.OperationTime;
                activity.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }

        public async Task<List<Activity>> GetAllActivities(CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                List<Activity> result = await db.Activities.Where(x => x.IsDeleted == false && x.ActivityBeginTime <= DateTime.Now && x.ActivityEndTime > DateTime.Now).ToListAsync(token);
                return result;
            }
        }
    }
}
