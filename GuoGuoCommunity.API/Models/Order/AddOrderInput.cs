namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddOrderInput
    {
        /// <summary>
        /// 业主认证申请Id(getListId)
        /// </summary>
        public string ApplicationRecordId { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 地址Id
        /// </summary>
        public string AddressId { get; set; }
    }
}