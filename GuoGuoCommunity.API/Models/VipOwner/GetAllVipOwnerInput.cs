using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllVipOwnerInput
    {
        /// <summary>
        /// 业委会名称(自动生成)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注名称
        /// </summary>
        public string RemarkName { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }
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