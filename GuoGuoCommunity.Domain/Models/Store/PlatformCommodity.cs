using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 平台商品
    /// </summary>
    public class PlatformCommodity : IEntitity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品价格（平台指导价）
        /// </summary>
        public decimal Price { get; set; }
       
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
        public string LastOperationUserId { get; set; }
        public DateTimeOffset? LastOperationTime { get; set; }
    }
}
