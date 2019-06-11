namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllShoppingTrollerInput
    {
        /// <summary>
        /// 店铺Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 用户认证ID
        /// </summary>
        public string OwnerCertificationRecordId { get; set; }

    }
}