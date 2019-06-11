using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    public class SmallDistrictShop : IEntitity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        [ForeignKey("SmallDistrict")]
        public Guid SmallDistrictId { get; set; }

        public SmallDistrict SmallDistrict { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        [ForeignKey("Shop")]
        public Guid ShopId { get; set; }

        public Shop Shop { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public decimal Postage { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }
    }
}
