using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllComplaintInput
    {
        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageSize { get; set; }
    }
}