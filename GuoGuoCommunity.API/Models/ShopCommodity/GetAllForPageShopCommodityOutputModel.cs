using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageShopCommodityOutputModel
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 商品类别
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 商品类别名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 销售状态
        /// </summary>
        public string SalesTypeValue { get; set; }

        /// <summary>
        /// 销售状态名称
        /// </summary>
        public string SalesTypeName { get; set; }
    }
}