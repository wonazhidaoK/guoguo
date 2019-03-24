using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
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
        public string Votes { get; set; }

        public string LastOperationUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? LastOperationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CreateOperationUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? CreateOperationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? DeletedTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
