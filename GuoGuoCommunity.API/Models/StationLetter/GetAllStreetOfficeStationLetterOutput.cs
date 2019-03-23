using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllStreetOfficeStationLetterOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetStreetOfficeStationLetterOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}