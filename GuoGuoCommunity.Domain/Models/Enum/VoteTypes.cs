using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 投票类型
    /// </summary>
    public class VoteTypes
    {
        static VoteTypes()
        {
            RecallProperty = new VoteTypes { Name = "发起倡议", Value = "RecallProperty" };
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

        public static IEnumerable<VoteTypes> GetAllForStreetOffice() => new List<VoteTypes>() { RecallProperty, Ordinary, VipOwnerElection };
    }
}
