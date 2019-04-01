using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 投票状态
    /// </summary>
    public class VoteStatus
    {
        static VoteStatus()
        {
            Processing = new VoteStatus { Name = "进行中", Value = "Processing" };
            Closed = new VoteStatus { Name = "已关闭", Value = "Closed" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 进行中
        /// </summary>
        public static VoteStatus Processing { get; set; }

        /// <summary>
        /// 已关闭
        /// </summary>
        public static VoteStatus Closed { get; set; }

        public static IEnumerable<VoteStatus> GetAll() => new List<VoteStatus>() { Processing, Closed };
    }
}
