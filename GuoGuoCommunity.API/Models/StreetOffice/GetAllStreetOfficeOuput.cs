using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllStreetOfficeOuput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetCommunityOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
       
    }
}