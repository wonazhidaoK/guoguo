using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllSmallDistrictOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetSmallDistrictOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }

    }
}