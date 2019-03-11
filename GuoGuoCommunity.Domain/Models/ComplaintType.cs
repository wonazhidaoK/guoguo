using GuoGuoCommunity.Domain.Abstractions.Models;
using System;

namespace GuoGuoCommunity.Domain.Models
{
    public class ComplaintType : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 投诉名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 投诉描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 投诉级别
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 发起部门名称
        /// </summary>
        public string InitiatingDepartmentName { get; set; }

        /// <summary>
        /// 发起部门值
        /// </summary>
        public string InitiatingDepartmentValue { get; set; }

        /// <summary>
        /// 处理期限
        /// </summary>
        public string ProcessingPeriod { get; set; }

        /// <summary>
        /// 申诉期限
        /// </summary>
        public string ComplaintPeriod { get; set; }

        public string LastOperationUserId { get; set; }
        public DateTimeOffset? LastOperationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
