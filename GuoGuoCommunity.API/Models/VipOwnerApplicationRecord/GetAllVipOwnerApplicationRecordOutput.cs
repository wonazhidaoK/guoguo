using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllVipOwnerApplicationRecordOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetVipOwnerApplicationRecordOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}