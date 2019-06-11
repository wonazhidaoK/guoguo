using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Dto
{
    public class GoodsTypeDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 商店Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

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
    public class GoodsTypeForPageDto
    {
        public List<GoodsType> List { get; set; }

        public int Count { get; set; }
    }
}
