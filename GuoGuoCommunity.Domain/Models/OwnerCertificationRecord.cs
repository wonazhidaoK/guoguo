using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业主认证记录
    /// </summary>
    public class OwnerCertificationRecord : IEntitity, IOwner, IIndustry
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        #region 基础信息

        /// <summary>
        /// 业户Id
        /// </summary>
        [ForeignKey("Industry")]
        public Guid IndustryId { get; set; }

        public Industry Industry { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        [ForeignKey("Owner")]
        public Guid? OwnerId { get; set; }

        public Owner Owner { get; set; }

        #endregion

        /// <summary>
        /// 认证时间
        /// </summary>
        public DateTimeOffset? CertificationTime { get; set; }

        /// <summary>
        /// 认证结果
        /// </summary>
        public string CertificationResult { get; set; }

        /// <summary>
        /// 认证状态1.认证中2.认证通过3.认证失败
        /// </summary>
        public string CertificationStatusName { get; set; }

        /// <summary>
        /// 认证状态值
        /// </summary>
        public string CertificationStatusValue { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsInvalid { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
    }
}
