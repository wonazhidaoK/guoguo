using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllStreetOfficeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetStreetOfficeOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
       
    }
}