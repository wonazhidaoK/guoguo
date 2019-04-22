using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VoteRecordDetailDto
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
        /// 投票选项ID
        /// </summary>
        public string VoteQuestionOptionId { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }
    }
}
