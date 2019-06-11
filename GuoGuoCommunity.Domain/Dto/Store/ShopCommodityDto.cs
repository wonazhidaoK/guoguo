using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Dto
{
    public class ShopCommodityDto
    {
        public string ShopId { get; set; }
        public string Id { get; set; }

        /// <summary>
        /// 商品类别外键关联
        /// </summary>
        public string TypeId { get; set; }

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
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品折扣价
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int CommodityStocks { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 销售状态名称
        /// </summary>
        public string SalesTypeName { get; set; }

        /// <summary>
        /// 销售状态值
        /// </summary>
        public string SalesTypeValue { get; set; }

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

    public class ShopCommodityForPageDto
    {
        public List<ShopCommodity> List { get; set; }

        public int Count { get; set; }
    }
}
