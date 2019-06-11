using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllShopOutput
    {

        /// <summary>
        /// 
        /// </summary>
        public List<GetAllShopOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllShopOutputModel
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
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 商超类别值
        /// </summary>
        public string MerchantCategoryValue { get; set; }

        /// <summary>
        /// 商超类别值
        /// </summary>
        public string MerchantCategoryName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}