namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageShopCommodityInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string ShopId { get; set; }
        /// <summary>
        /// 商品条形码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品类别
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 销售状态
        /// </summary>
        public string SalesTypeValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }
    }
}