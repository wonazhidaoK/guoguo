using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业委会架构
    /// </summary>
    public class VipOwnerStructure : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 职能名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 职能权重
        /// </summary>
        public string Weights { get; set; }

        /// <summary>
        /// 是否具有审核权限
        /// </summary>
        public bool IsReview { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public string LastOperationUserId { get; set; }
        public DateTimeOffset? LastOperationTime { get; set; }
        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
