namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetListForShopUserOutput
    {
        /// <summary>
        /// 店铺商品ID（外键）
        /// </summary>
        public string CommodityId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string CommodityName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string CommodityImageUrl { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal CommodityPrice { get; set; }

        /// <summary>
        /// 商品折扣价
        /// </summary>
        public decimal? DiscountPrice { get; set; }
    }
}