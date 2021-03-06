﻿using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投诉跟进
    /// </summary>
    public class ComplaintFollowUp : IEntitity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 投诉Id
        /// </summary>
        [Required]
        [ForeignKey("Complaint")]
        public Guid ComplaintId { get; set; }

        public Complaint Complaint { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作人部门名称
        /// </summary>
        public string OperationDepartmentName { get; set; }

        /// <summary>
        /// 操作人部门值
        /// </summary>
        public string OperationDepartmentValue { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        [ForeignKey("OwnerCertificationRecord")]
        public Guid? OwnerCertificationRecordId { get; set; }

        public OwnerCertificationRecord OwnerCertificationRecord { get; set; }

        /// <summary>
        /// 是否是申诉操作
        /// </summary>
        public string Aappeal { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
