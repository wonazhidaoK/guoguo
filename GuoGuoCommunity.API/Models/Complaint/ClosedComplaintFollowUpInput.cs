namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ClosedComplaintFollowUpInput
    {
        /// <summary>
        /// 投诉Id
        /// </summary>
        public string ComplaintId { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }
    }
}