using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllPlatformCommodityOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetPlatformCommodityOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}