using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForHistoryVipOwnerApplicationRecordOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetForHistoryVipOwnerApplicationRecordOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}