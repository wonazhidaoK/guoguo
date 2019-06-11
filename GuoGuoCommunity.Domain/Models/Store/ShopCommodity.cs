using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 店铺商品
    /// </summary>
    public class ShopCommodity : IEntitity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 商品类别外键关联
        /// </summary>
        [Required]
        [ForeignKey("GoodsType")]
        public Guid TypeId { get; set; }

        public GoodsType GoodsType { get; set; }

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
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品折扣价
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int CommodityStocks { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 销售状态名称
        /// </summary>
        public string SalesTypeName { get; set; }

        /// <summary>
        /// 销售状态值
        /// </summary>
        public string SalesTypeValue { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
        public string LastOperationUserId { get; set; }
        public DateTimeOffset? LastOperationTime { get; set; }
    }
}
