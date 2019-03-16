using System;

namespace GuoGuoCommunity.Domain.Dto
{
    class VipOwnerCertificationAnnexDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 申请条件Id
        /// </summary>
        public string CertificationConditionId { get; set; }

        /// <summary>
        /// 申请Id
        /// </summary>
        public string ApplicationRecordId { get; set; }

        /// <summary>
        /// 上传Id
        /// </summary>
        public string UploadId { get; set; }

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
