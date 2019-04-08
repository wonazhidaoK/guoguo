using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>     
    /// 
    /// </summary>
    public class GetAllComplaintForStreetOfficeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetComplaintOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}