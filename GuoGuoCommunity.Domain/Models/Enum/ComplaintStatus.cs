using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 投诉状态
    /// </summary>
    public class ComplaintStatus
    {
        static ComplaintStatus()
        {
            NotAccepted = new ComplaintStatus { Name = "未授理", Value = "NotAccepted" };
            Processing = new ComplaintStatus { Name = "处理中", Value = "Processing" };
            Finished = new ComplaintStatus { Name = "已完结", Value = "Finished" };
            Completed = new ComplaintStatus { Name = "已完成", Value = "Completed" };
            StreetOfficeNotAccepted= new ComplaintStatus { Name = "街道办未授理", Value = "StreetOfficeNotAccepted" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 未受理
        /// </summary>
        public static ComplaintStatus NotAccepted { get; set; }

        /// <summary>
        /// 处理中
        /// </summary>
        public static ComplaintStatus Processing { get; set; }

        /// <summary>
        /// 已完结
        /// </summary>
        public static ComplaintStatus Finished { get; set; }

        /// <summary>
        /// 街道办未受理
        /// </summary>
        public static ComplaintStatus StreetOfficeNotAccepted { get; set; }

        /// <summary>
        /// 街道办处理中
        /// </summary>
        public static ComplaintStatus StreetOfficeProcessing { get; set; }

        /// <summary>
        /// 街道办已完结
        /// </summary>
        public static ComplaintStatus StreetOfficeFinished { get; set; }

        /// <summary>
        /// 已完成
        /// </summary>
        public static ComplaintStatus Completed { get; set; }

        public static IEnumerable<ComplaintStatus> GetAll() => new List<ComplaintStatus>() { NotAccepted, Processing, Finished, Completed };
    }
}
