namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddSmallDistrictShopInput
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public decimal Postage { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}