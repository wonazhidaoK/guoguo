using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VoteQuestionOptionDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 投票Id
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 投票问题Id
        /// </summary>
        public string VoteQuestionId { get; set; }

        /// <summary>
        /// 选项描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 票数
        /// </summary>
        public string Votes { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
