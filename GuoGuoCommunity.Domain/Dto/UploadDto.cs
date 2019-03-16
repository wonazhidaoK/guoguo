using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class UploadDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 协议
        /// </summary>
        public string Agreement { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 一级域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 业务目录
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// 动态文件地址
        /// </summary>
        public string File { get; set; }

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
