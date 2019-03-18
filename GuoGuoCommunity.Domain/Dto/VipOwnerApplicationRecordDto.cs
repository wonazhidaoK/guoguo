using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VipOwnerApplicationRecordDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 申请人Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 职能Id
        /// </summary>
        public string StructureId { get; set; }

        /// <summary>
        /// 职能名称
        /// </summary>
        public string StructureName { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public string IsInvalid { get; set; }

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
