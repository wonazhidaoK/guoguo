using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForShopUserSmallDistrictShopOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllForShopUserSmallDistrictShopOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllForShopUserSmallDistrictShopOutputModel
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
        /// 商家手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Logo图片Url
        /// </summary>
        public string LogoImageUrl { get; set; }

        public string ShopId { get; set; }
    }
}