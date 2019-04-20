using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPropertyInput
    {
        /// <summary>
        /// 发布时间（开始时间）
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 发布时间（结束时间）
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 投票状态
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

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