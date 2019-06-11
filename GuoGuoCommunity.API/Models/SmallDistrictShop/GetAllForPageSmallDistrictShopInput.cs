namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageSmallDistrictShopInput
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }
    }
}