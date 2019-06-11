using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    public  class OrderItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public Order  Order { get; set; }

        [Required]
        [ForeignKey("ShopCommodity")]
        public Guid ShopCommodityId { get; set; }

        public ShopCommodity  ShopCommodity { get; set; }

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
        public decimal? DiscountPrice { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }
    }
}
