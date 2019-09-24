using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Dto.Store
{
    public class ActivityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 满减条件金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 满减优惠价格
        /// </summary>
        public decimal Off { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public string ShopId { get; set; }
        /// <summary>
        /// 活动类型
        /// </summary>
        public int ActivityType { get; set; }
        /// <summary>
        /// 活动来源 1店铺，2平台
        /// </summary>
        public int ActivitySource { get; set; }

        public DateTime ActivityBeginTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActivityEndTime { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
        /// <summary>
        /// 是否按时间条件查询
        /// </summary>
        public bool IsSelectByTime { get; set; }

    }

    public enum ActivityTypeEnum
    {
        //满减
        MoneyOff = 1,
        //折扣
        Discount = 2
    }
    public enum ActivitySourceEnum
    {
        //店铺活动
        ShopMoneyOffActivity = 1,
        //平台活动
        PlatformActivity = 2
    }
}
