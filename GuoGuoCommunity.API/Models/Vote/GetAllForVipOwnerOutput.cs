using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForVipOwnerOutput
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