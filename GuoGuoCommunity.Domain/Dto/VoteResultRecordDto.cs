using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VoteResultRecordDto
    {
        public string Id { get; set; }

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

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
