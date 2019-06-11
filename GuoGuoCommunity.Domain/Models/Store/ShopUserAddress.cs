using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 商超用户地址
    /// </summary>
    public class ShopUserAddress: IEntitity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 申请Id
        /// </summary>
        [Required]
       
        public Guid ApplicationRecordId { get; set; }

        [ForeignKey("ApplicationRecordId")]
        public OwnerCertificationRecord OwnerCertificationRecord { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 业户Id
        /// </summary>
        [ForeignKey("Industry")]
        public Guid IndustryId { get; set; }

        [ForeignKey("IndustryId")]
        public Industry Industry { get; set; }

        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }
    }
}
