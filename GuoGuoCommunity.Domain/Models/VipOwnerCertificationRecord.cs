using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业委会成员认证记录
    /// </summary>
    public class VipOwnerCertificationRecord : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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
        public bool IsInvalid { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
