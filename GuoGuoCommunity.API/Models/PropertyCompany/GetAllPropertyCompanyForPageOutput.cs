using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllPropertyCompanyForPageOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllPropertyCompanyForPageOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}