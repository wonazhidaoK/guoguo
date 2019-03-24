﻿using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllPropertyStationLetterInput
    {
        /// <summary>
        /// 小区范围
        /// </summary>
        public string SmallDistrict { get; set; }

        ///// <summary>
        ///// 发布时间（开始时间）
        ///// </summary>
        //public DateTimeOffset? ReleaseTimeStart { get; set; }

        ///// <summary>
        ///// 发布时间（结束时间）
        ///// </summary>
        //public DateTimeOffset? ReleaseTimeEnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageSize { get; set; }
    }
}