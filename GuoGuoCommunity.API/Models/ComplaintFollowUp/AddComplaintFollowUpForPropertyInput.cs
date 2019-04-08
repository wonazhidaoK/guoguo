namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddComplaintFollowUpForPropertyInput
    {
        /// <summary>
        /// 投诉Id
        /// </summary>
        public string ComplaintId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }
    }
}