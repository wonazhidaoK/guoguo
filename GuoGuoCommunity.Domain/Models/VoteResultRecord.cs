using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 投票结果记录
    /// </summary>
    public class VoteResultRecord : IDeleted, ILastOperation, ICreateOperation
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
        /// 结果计算方式值
        /// </summary>
        public string CalculationMethodValue { get; set; }

        /// <summary>
        /// 结果计算方式名称
        /// </summary>
        public string CalculationMethodName { get; set; }

        /// <summary>
        /// 投票结果值
        /// </summary>
        public string ResultValue { get; set; }

        /// <summary>
        /// 投票结果名称
        /// </summary>
        public string ResultName { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
    }
}
