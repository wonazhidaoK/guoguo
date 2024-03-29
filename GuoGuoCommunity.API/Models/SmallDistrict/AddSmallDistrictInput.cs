﻿using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddSmallDistrictInput
    {
        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Required]
        public string Region { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 街道办Id
        /// </summary>
        [Required]
        public string StreetOfficeId { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// 社区Id
        /// </summary>
        [Required]
        public string CommunityId { get; set; }

        /// <summary>
        /// 物业公司Id
        /// </summary>
        public string PropertyCompanyId { get; set; }
    }
}