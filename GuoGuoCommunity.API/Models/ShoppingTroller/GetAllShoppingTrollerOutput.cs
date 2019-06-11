using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllShoppingTrollerOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllShoppingTrollerOutputModel> List { get; set; }

        /// <summary>
        /// 商品总件数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 商品总价格
        /// </summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllShoppingTrollerOutputModel
    {
        /// <summary>
        /// 店铺商品ID（外键）
        /// </summary>
        public string CommodityId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string CommodityName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string CommodityImageUrl { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal CommodityPrice { get; set; }

        
    }
}