using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateComplaintTypeInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 投诉名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 投诉描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 投诉级别
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 发起部门名称
        /// </summary>
        public string InitiatingDepartmentName { get; set; }

        /// <summary>
        /// 发起部门值
        /// </summary>
        public string InitiatingDepartmentValue { get; set; }
    }
}