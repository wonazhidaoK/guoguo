using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{

    /// <summary>
    /// 业委会认证附件表
    /// </summary>
    public class VipOwnerCertificationAnnex : ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 申请条件Id
        /// </summary>
        public string CertificationConditionId { get; set; }

        /// <summary>
        /// 申请Id
        /// </summary>
        public string ApplicationRecordId { get; set; }

        /// <summary>
        /// 上传Id
        /// </summary>
        public string UploadId { get; set; }

        

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime  { get; set; }

        
    }
}
