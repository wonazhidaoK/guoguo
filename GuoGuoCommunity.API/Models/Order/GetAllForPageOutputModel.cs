using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllForPageOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageOutputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string OrderStatusName { get; set; }

        /// <summary>
        /// 订单状态值
        /// </summary>
        public string OrderStatusValue { get; set; }

        /// <summary>
        /// 付款状态名称
        /// </summary>
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 付款状态值
        /// </summary>
        public string PaymentStatusValue { get; set; }

        #region 配送方相关

        /// <summary>
        /// 配送方电话
        /// </summary>
        public string DeliveryPhone { get; set; }

        /// <summary>
        /// 配送方名称
        /// </summary>
        public string DeliveryName { get; set; }

        #endregion

        /// <summary>
        /// 订单编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 小区商铺关联id
        /// </summary>
        public string SmallDistrictShopId { get; set; }

        /// <summary>
        /// 店铺邮费
        /// </summary>
        public decimal Postage { get; set; }

        /// <summary>
        /// 店铺Logo
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 订单商品总件数
        /// </summary>
        public int ShopCommodityCount { get; set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal ShopCommodityPrice { get; set; }

        /// <summary>
        /// 应付款金额
        /// </summary>
        public decimal PaymentPrice { get; set; }

        /// <summary>
        /// 商品列表
        /// </summary>
        public List<OrdeItemModel> List { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OrdeItemModel
    {
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
        public decimal? DiscountPrice { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int CommodityCount { get; set; }

        /// <summary>
        /// 折扣价格
        /// </summary>
        public decimal OriginalPrice { get; set; }
    }
}