using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllComplaintOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public  List<GetComplaintOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 未处理数量
        /// </summary>
        public int NotAcceptedCount { get; set; }
        /// <summary>
        /// 处理中数量
        /// </summary>
        public int ProcessingCount { get; set; }

        /// <summary>
        /// 已完结数量
        /// </summary>
        public int FinishedCount { get; set; }

        /// <summary>
        /// 已完结数量
        /// </summary>
        public int CompletedCount { get; set; }
    }
}