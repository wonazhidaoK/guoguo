﻿using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetComplaintFollowUpListOutput
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public string Id { get; set; }

        ///// <summary>
        ///// 投诉Id
        ///// </summary>
        //public string ComplaintId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作人部门名称
        /// </summary>
        public string OperationDepartmentName { get; set; }

        ///// <summary>
        ///// 操作人部门值
        ///// </summary>
        //public string OperationDepartmentValue { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        ///// <summary>
        ///// 业主认证Id
        ///// </summary>
        //public string OwnerCertificationId { get; set; }

        /// <summary>
        /// 是否是申诉操作
        /// </summary>
        public string Aappeal { get; set; }

        /// <summary>
        /// 附件Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否是创建人
        /// </summary>
        public bool IsCreateUser { get; set; }
    }
}