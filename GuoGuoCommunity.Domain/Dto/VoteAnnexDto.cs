using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VoteAnnexDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 投票Id
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        ///  附件内容
        /// </summary>
        public string AnnexContent { get; set; }

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
