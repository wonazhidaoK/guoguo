using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageSmallDistrictShopOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllForPageSmallDistrictShopOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageSmallDistrictShopOutputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public decimal Postage { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}