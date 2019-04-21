using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetComplaintFollowUpOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetComplaintFollowUpListOutput>  List { get; set; }

        /// <summary>
        /// 过期时间(申诉截至时间)
        /// </summary>
        public DateTimeOffset ExpiredTime { get; set; }

        /// <summary>
        /// 申诉开始日期
        /// </summary>
        public DateTimeOffset ProcessUpTime { get; set; }

        /// <summary>
        /// 处理期限
        /// </summary>
        public int ProcessingPeriod { get; set; }

        /// <summary>
        /// 申诉期限
        /// </summary>
        public int ComplaintPeriod { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 投诉类型名称
        /// </summary>
        public string ComplaintTypeName { get; set; }
    }
}