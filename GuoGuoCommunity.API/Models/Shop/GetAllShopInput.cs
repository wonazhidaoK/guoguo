using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllShopInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 商超类别值
        /// </summary>
        public string MerchantCategoryValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageSize { get; set; }
    }
}