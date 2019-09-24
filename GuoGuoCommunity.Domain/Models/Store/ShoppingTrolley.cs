using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models.Store
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingTrolley : ILastOperation
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 店铺商品ID（外键）
        /// </summary>
        [Required]
        [ForeignKey("ShopCommodity")]
        public Guid ShopCommodityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ShopCommodity ShopCommodity { get; set; }

        ///// <summary>
        ///// 用户认证ID（外键）
        ///// </summary>
        //[Required]
        //[ForeignKey("OwnerCertificationRecord")]
        //public Guid OwnerCertificationRecordId { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public OwnerCertificationRecord OwnerCertificationRecord { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        [ForeignKey("CreateOperationUserId")]
        public User User { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
