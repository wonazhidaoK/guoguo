using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class ComplaintTypeDto
    {
        public string Id { get; set; }

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
