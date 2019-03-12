namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetVipOwnerOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 业委会名称(自动生成)
        /// </summary>
        public string Name { get; set; }

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

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
    }
}