using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Dto
{
    public class SmallDistrictShopDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public decimal Postage { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public class SmallDistrictShopForPageDto
    {
        public List<SmallDistrictShop> List { get; set; }

        public int Count { get; set; }
    }
}
