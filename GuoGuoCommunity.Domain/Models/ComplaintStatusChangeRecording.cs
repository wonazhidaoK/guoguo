using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投诉记录变更记录
    /// </summary>
    public class ComplaintStatusChangeRecording : ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public string OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public string NewStatus { get; set; }

        /// <summary>
        /// 投诉跟进id
        /// </summary>
        [ForeignKey("ComplaintFollowUp")]
        public Guid ComplaintFollowUpId { get; set; }

        public ComplaintFollowUp ComplaintFollowUp { get; set; }

        ///// <summary>
        ///// 投诉Id
        ///// </summary>
        //public string ComplaintId { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
