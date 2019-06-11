using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投诉
    /// </summary>
    public class Complaint : IEntitity, IOwnerCertificationRecord, IComplaintType
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 投诉类型Id
        /// </summary>
        [Required]
        [ForeignKey("ComplaintType")]
        public Guid ComplaintTypeId { get; set; }

        public ComplaintType ComplaintType { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        [Required]
        [ForeignKey("OwnerCertificationRecord")]
        public Guid OwnerCertificationRecordId { get; set; }

        public OwnerCertificationRecord OwnerCertificationRecord { get; set; }

        /// <summary>
        /// 投诉关闭时间
        /// </summary>
        public DateTimeOffset? ClosedTime { get; set; }

        /// <summary>
        /// 过期时间(申诉截至时间)
        /// </summary>
        public DateTimeOffset? ExpiredTime { get; set; }

        /// <summary>
        /// 处理截至时间
        /// </summary>
        public DateTimeOffset? ProcessUpTime { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public string IsInvalid { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// 创建操作人部门名称
        /// </summary>
        public string OperationDepartmentName { get; set; }

        /// <summary>
        /// 创建操作人部门值
        /// </summary>
        public string OperationDepartmentValue { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
