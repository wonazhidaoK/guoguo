namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddPlatformCommodityInput
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
        /// 商品图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品价格（平台指导价）
        /// </summary>
        public decimal Price { get; set; }
    }
}