using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain
{
    /// <summary>
    /// 业主申请附件
    /// </summary>
    public class VipOwnerApplicationAnnex : ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 申请Id
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// 申请条件Id
        /// </summary>
        public string ConditionId { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }

        /// <summary>
        /// 附件内容
        /// </summary>
        public string AnnexContent { get; set; }

        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
