using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllPlatformCommodityInput
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string BarCode { get; set; }

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