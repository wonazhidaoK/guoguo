using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 投票结果
    /// </summary>
    public class VoteResult
    {
        static VoteResult()
        {
            Adopt = new VoteResult { Name = "通过", Value = "Adopt" };
            Overrule = new VoteResult { Name = "未通过", Value = "Overrule" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 通过
        /// </summary>
        public static VoteResult Adopt { get; set; }

        /// <summary>
        /// 未通过
        /// </summary>
        public static VoteResult Overrule { get; set; }

        public static IEnumerable<VoteResult> GetAll() => new List<VoteResult>() { Adopt, Overrule };
    }
}
