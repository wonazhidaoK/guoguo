using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业委会成员申请记录
    /// </summary>
    public class VipOwnerApplicationRecord : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
