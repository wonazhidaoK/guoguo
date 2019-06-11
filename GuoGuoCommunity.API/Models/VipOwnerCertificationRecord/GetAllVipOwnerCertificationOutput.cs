using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllVipOwnerCertificationOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetVipOwnerCertificationOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}