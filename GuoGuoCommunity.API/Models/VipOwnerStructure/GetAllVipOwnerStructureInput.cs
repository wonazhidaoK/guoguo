using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllVipOwnerStructureInput
    {
        /// <summary>
        /// 职能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职能权重
        /// </summary>
        public string Weights { get; set; }

        /// <summary>
        /// 是否具有审核权限
        /// </summary>
        public bool? IsReview { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

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