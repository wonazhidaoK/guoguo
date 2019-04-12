using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 投票记录
    /// </summary>
    public class AddIndefiniteVoteRecordIntput
    {
        /// <summary>
        /// 投票Id
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 反馈
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }

        /// <summary>
        /// 投票详情
        /// </summary>
        public List<AddIndefiniteVoteRecordDetailInput> List { get; set; }
    }

    /// <summary>
    /// 投票记录详情
    /// </summary>
    public class AddIndefiniteVoteRecordDetailInput
    {
        /// <summary>
        /// 投票问题Id
        /// </summary>
        public string VoteQuestionId { get; set; }

        /// <summary>
        /// 投票选项ID
        /// </summary>
        public List<string> VoteQuestionOptionId { get; set; }
    }
}