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

        ///// <summary>
        ///// 投诉Id
        ///// </summary>
        //[Required]
        //[ForeignKey("Complaint")]
        //public Guid ComplaintId { get; set; }

        //public Complaint Complaint { get; set; }

        /// <summary>
        /// 投诉跟进Id
        /// </summary>
        [ForeignKey("ComplaintFollowUp")]
        public Guid? ComplaintFollowUpId { get; set; }

        public ComplaintFollowUp ComplaintFollowUp { get; set; }

        /// <summary>
        ///  附件内容
        /// </summary>
        public string AnnexContent { get; set; }

        /// <summary>
        /// 附件id(当附件为文件时保存附件Id)
        /// </summary>
        [Required]
        [ForeignKey("Upload")]
        public Guid AnnexId { get; set; }

        public Upload Upload { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
