﻿using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetStationLetterOutput
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// 附件url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? ReleaseTime { get; set; }
    }
}