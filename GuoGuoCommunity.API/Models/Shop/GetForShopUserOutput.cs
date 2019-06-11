namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetForShopUserOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// Logo图片url
        /// </summary>
        public string LogoImageUrl { get; set; }

        /// <summary>
        /// 购物车内是否存在商品
        /// </summary>
        public bool IsPresence { get; set; }

        /// <summary>
        /// 配送费
        /// </summary>
        public decimal Postage { get; set; }
    }
}