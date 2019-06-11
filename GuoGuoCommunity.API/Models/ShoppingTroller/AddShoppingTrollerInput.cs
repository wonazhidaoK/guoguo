namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddShoppingTrollerInput
    {
        /// <summary>
        /// 店铺商品ID（外键）
        /// </summary>
        public string ShopCommodityId { get; set; }

        /// <summary>
        /// 用户认证ID（外键）
        /// </summary>
        public string OwnerCertificationRecordId { get; set; }

        /// <summary>
        /// 商品数量(商品更改数量)
        /// </summary>
        public int CommodityCount { get; set; }
    }
}