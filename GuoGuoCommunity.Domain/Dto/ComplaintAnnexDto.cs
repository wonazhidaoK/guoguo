using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class ComplaintAnnexDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 投诉Id
        /// </summary>
        public string ComplaintId { get; set; }

        /// <summary>
        /// 投诉跟进Id
        /// </summary>
        public string ComplaintFollowUpId { get; set; }

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
