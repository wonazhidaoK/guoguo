using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VipOwnerStructureDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 职能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职能权重
        /// </summary>
        public string Weights { get; set; }

        /// <summary>
        /// 是否具有审核权限
        /// </summary>
        public bool? IsReview { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

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
