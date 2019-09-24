namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateShoppingTrollerInput
    {
        /// <summary>
        /// 店铺商品ID
        /// </summary>
        public string ShopCommodityId { get; set; }

        ///// <summary>
        ///// 用户认证ID
        ///// </summary>
        //public string OwnerCertificationRecordId { get; set; }

        /// <summary>
        /// 商品变更数量
        /// </summary>
        public int CommodityCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeleteShopingTrollerInput
    {
        /// <summary>
        /// 店铺
        /// </summary>
        public string ShopId { get; set; }

        ///// <summary>
        ///// 用户认证ID
        ///// </summary>
        //public string OwnerCertificationRecordId { get; set; }
    }
}