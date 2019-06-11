using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Dto
{
    public class PlatformCommodityDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品价格（平台指导价）
        /// </summary>
        public decimal Price { get; set; }

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

        public int TotalCount { get; set; }
    }

    public class PlatformCommodityForPage
    {
        public List<PlatformCommodity> PlatformCommoditieForPageList { get; set; }

        public int Count { get; set; }
    }
}
