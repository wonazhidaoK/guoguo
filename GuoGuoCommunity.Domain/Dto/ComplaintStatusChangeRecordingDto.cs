using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class ComplaintStatusChangeRecordingDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public string OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public string NewStatus { get; set; }

        /// <summary>
        /// 投诉跟进id
        /// </summary>
        public string ComplaintFollowUpId { get; set; }

        /// <summary>
        /// 投诉Id
        /// </summary>
        public string ComplaintId { get; set; }

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
