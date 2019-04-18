using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投诉附件
    /// </summary>
    public class ComplaintAnnex : ICreateOperation
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
        public string ComplaintId { get; set; }

        /// <summary>
        /// 投诉跟进Id
        /// </summary>
        public string ComplaintFollowUpId { get; set; }

        /// <summary>
        ///  附件内容
        /// </summary>
        public string AnnexContent { get; set; }

        /// <summary>
        /// 附件id(当附件为文件时保存附件Id)
        /// </summary>
        public string AnnexId { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
