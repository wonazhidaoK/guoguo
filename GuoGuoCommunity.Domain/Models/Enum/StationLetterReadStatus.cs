using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 站内信阅读状态
    /// </summary>
    public class StationLetterReadStatus
    {
        static StationLetterReadStatus()
        {
            All = new StationLetterReadStatus { Name = "全部", Value = "All" };
            HaveRead = new StationLetterReadStatus { Name = "已读", Value = "HaveRead" };
            UnRead = new StationLetterReadStatus { Name = "未读", Value = "UnRead" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 全部
        /// </summary>
        public static StationLetterReadStatus All { get; set; }

        /// <summary>
        /// 已读
        /// </summary>
        public static StationLetterReadStatus HaveRead { get; set; }

        /// <summary>
        /// 未读
        /// </summary>
        public static StationLetterReadStatus UnRead { get; set; }

        public static IEnumerable<StationLetterReadStatus> GetAll() => new List<StationLetterReadStatus>() { All, HaveRead, UnRead };
    }
}
