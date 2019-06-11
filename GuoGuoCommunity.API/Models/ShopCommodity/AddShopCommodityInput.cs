namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddShopCommodityInput
    {
        /// <summary>
        /// 商品条形码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品折扣价
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 商品类别
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int CommodityStocks { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImageUrl { get; set; }
    }
}