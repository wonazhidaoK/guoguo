﻿using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public  class OwnerCertificationRecordDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// 认证时间
        /// </summary>
        public string CertificationTime { get; set; }

        /// <summary>
        /// 认证结果
        /// </summary>
        public string CertificationResult { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public string IsValid { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        /// <summary>
        /// 认证状态1.认证中2.认证通过3.认证失败
        /// </summary>
        public string CertificationStatusName { get; set; }

        /// <summary>
        /// 认证状态值
        /// </summary>
        public string CertificationStatusValue { get; set; }
    }
}
