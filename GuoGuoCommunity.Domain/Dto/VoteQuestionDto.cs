using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class VoteQuestionDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 投票Id
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 选项方式
        /// </summary>
        public string OptionMode { get; set; }

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
