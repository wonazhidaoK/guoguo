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

        /// <summary>
        /// 是否进行过选举
        /// </summary>
        public bool IsElection { get; set; }

        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool IsCanDeleted { get; set; }
        //是否可以置为无效

        /// <summary>
        /// 是否可以置为无效
        /// </summary>
        public bool IsCanInvalid { get; set; }
    }
}