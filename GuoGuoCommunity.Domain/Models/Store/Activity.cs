using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Models.Store
{
    public class Activity: IEntitity
    {
        /// <summary>
        /// 主键
        /// </summary>

        [Key]   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        /// <summary>
        /// 满减条件金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 满减优惠价格
        /// </summary>
        public decimal Off { get; set; }

        /// <summary>
        /// 店铺关联外键
        /// </summary>
        [Required]
        [ForeignKey("Shop")]
        public Guid ShopId { get; set; }

        public Shop Shop { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public int ActivityType { get; set; }
        /// <summary>
        /// 活动来源 1店铺，2平台
        /// </summary>
        public int ActivitySource { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActivityBeginTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActivityEndTime { get; set; }


        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }
    }

    
}
