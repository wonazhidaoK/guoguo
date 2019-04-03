using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetComplaintFollowUpOutput
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
    }
}