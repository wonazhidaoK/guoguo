﻿using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VipOwnerCertificationAnnexDto
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
        /// 附件内容
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
