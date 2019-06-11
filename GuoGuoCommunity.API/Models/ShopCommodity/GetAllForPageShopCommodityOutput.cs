using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageShopCommodityOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllForPageShopCommodityOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}