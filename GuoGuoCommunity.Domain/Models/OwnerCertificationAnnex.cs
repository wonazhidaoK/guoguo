using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业主认证附件表
    /// </summary>
    public class OwnerCertificationAnnex : ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 附件类型值
        /// </summary>
        public string OwnerCertificationAnnexTypeValue { get; set; }

        /// <summary>
        /// 申请Id
        /// </summary>
        [Required]
        [ForeignKey("OwnerCertificationRecord")]
        public Guid ApplicationRecordId { get; set; }

        public OwnerCertificationRecord OwnerCertificationRecord { get; set; }

        /// <summary>
        /// 附件内容
        /// </summary>
        public string AnnexContent { get; set; }

        /// <summary>
        /// 附件Id(当附件为文件时保存附件Id)
        /// </summary>
        [Required]
        [ForeignKey("Upload")]
        public Guid AnnexId { get; set; }

        public Upload Upload { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
