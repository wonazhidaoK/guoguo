﻿namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetComplaintTypeOutput
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

        /// <summary>
        /// 处理期限
        /// </summary>
        public int ProcessingPeriod { get; set; }

        /// <summary>
        /// 申诉期限
        /// </summary>
        public int ComplaintPeriod { get; set; }
    }
}