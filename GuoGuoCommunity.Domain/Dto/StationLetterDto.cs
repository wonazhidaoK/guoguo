using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class StationLetterDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 小区范围
        /// </summary>
        public string SmallDistrictArray { get; set; }

        /// <summary>
        /// 街道办Id
        /// </summary>
        public string StreetOfficeId { get; set; }

        /// <summary>
        /// 街道办名称
        /// </summary>
        public string StreetOfficeName { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        /// <summary>
        /// 读取状态
        /// </summary>
        public string ReadStatus { get; set; }
        /// <summary>
        /// 发布时间（开始时间）
        /// </summary>
        public DateTimeOffset? ReleaseTimeStart { get; set; }

        /// <summary>
        /// 发布时间（结束时间）
        /// </summary>
        public DateTimeOffset? ReleaseTimeEnd { get; set; }
    }
}
