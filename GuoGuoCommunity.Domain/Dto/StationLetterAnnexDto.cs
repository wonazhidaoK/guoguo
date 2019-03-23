using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class StationLetterAnnexDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 站内信Id
        /// </summary>
        public string StationLetterId { get; set; }

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
