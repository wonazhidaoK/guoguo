using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetFeedbackOutput
    {
        /// <summary>
        /// 建议
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset ReleaseTime { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperationName { get; set; }
    }
}