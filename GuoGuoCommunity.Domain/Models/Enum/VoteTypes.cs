using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 投票状态
    /// </summary>
    public class VoteTypes
    {
        static VoteTypes()
        {
            RecallProperty = new VoteTypes { Name = "罢免物业", Value = "RecallProperty" };
            Ordinary = new VoteTypes { Name = "普通投票", Value = "Ordinary" };
            VipOwnerElection = new VoteTypes { Name = "业委会重组", Value = "VipOwnerElection" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 罢免物业
        /// </summary>
        public static VoteTypes RecallProperty { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public static VoteTypes Ordinary { get; set; }

        /// <summary>
        /// 业委会重组
        /// </summary>
        public static VoteTypes VipOwnerElection { get; set; }

        public static IEnumerable<VoteTypes> GetAll() => new List<VoteTypes>() { RecallProperty, Ordinary, VipOwnerElection };

        public static IEnumerable<VoteTypes> GetAllForVipOwner() => new List<VoteTypes>() { RecallProperty, Ordinary };
    }
}
