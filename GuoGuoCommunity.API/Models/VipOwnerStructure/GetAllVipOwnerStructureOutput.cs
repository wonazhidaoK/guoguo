using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllVipOwnerStructureOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetVipOwnerStructureOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}