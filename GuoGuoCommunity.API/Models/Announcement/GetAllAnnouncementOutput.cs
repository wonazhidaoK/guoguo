using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllAnnouncementOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetVipOwnerAnnouncementOutput> List { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}