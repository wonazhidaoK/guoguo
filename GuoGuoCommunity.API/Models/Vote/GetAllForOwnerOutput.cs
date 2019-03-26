using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models.Vote
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForOwnerOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetForStreetOfficeOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}