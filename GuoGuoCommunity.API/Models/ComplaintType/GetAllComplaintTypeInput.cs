using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllComplaintTypeInput
    {
        /// <summary>
        /// 投诉名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 投诉描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 发起部门值
        /// </summary>
        public string InitiatingDepartmentValue { get; set; }

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