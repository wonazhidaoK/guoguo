﻿using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VipOwnerCertificationRecordDto
    {

        public string Id { get; set; }

        /// <summary>
        /// 业委会Id
        /// </summary>
        public string VipOwnerId { get; set; }

        /// <summary>
        /// 业委会名称
        /// </summary>
        public string VipOwnerName { get; set; }

        /// <summary>
        /// 业委会职能Id
        /// </summary>
        public string VipOwnerStructureId { get; set; }

        /// <summary>
        /// 业委会职能名称
        /// </summary>
        public string VipOwnerStructureName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public string IsInvalid { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        /// <summary>
        /// 投票id
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 操作人小区Id
        /// </summary>
        public string OperationUserSmallDistrictId { get; set; }

        /// <summary>
        /// 创建时间（开始时间）
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        ///  创建时间（结束时间）
        /// </summary>
        public DateTimeOffset EndTime { get; set; }
    }
}
