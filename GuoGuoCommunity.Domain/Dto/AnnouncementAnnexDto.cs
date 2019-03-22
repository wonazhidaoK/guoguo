using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class AnnouncementAnnexDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 公告Id
        /// </summary>
        public string AnnouncementId { get; set; }

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
