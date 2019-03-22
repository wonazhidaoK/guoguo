namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddVipOwnerAnnouncementInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 小区范围
        /// </summary>
        public string SmallDistrict { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }
    }
}