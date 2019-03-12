namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddVipOwnerInput
    {
        /// <summary>
        /// 备注名称
        /// </summary>
        public string RemarkName { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string SmallDistrictName { get; set; }
    }
}