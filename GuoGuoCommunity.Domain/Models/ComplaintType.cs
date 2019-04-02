using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投诉类别
    /// </summary>
    public class ComplaintType : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 投诉名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 投诉描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 投诉级别
        /// </summary>
        [Required]
        public string Level { get; set; }

        /// <summary>
        /// 发起部门名称
        /// </summary>
        [Required]
        public string InitiatingDepartmentName { get; set; }

        /// <summary>
        /// 发起部门值
        /// </summary>
        [Required]
        public string InitiatingDepartmentValue { get; set; }

        /// <summary>
        /// 处理期限
        /// </summary>
        [Required]
        public int ProcessingPeriod { get; set; }

        /// <summary>
        /// 申诉期限
        /// </summary>
        [Required]
        public int ComplaintPeriod { get; set; }

        public string LastOperationUserId { get; set; }
        public DateTimeOffset? LastOperationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
