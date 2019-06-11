namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddShopUserAddressInput
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 业户Id
        /// </summary>
        public string IndustryId { get; set; }

        /// <summary>
        /// 业主认证申请Id(getListId)
        /// </summary>
        public string ApplicationRecordId { get; set; }
    }
}