namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateShopUserAddressInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

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
    }
}