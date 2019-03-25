using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投票问题选项
    /// </summary>
    public class VoteQuestionOption : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        /// <summary>
        /// 投票Id
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 投票问题Id
        /// </summary>
        public string VoteQuestionId { get; set; }

        /// <summary>
        /// 选项描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 票数
        /// </summary>
        public int Votes { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
    }
}
