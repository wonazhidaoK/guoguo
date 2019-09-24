using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    public class Activity
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

    }
}