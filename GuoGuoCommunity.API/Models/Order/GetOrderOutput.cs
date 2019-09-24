using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetOrderOutput
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

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }


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
        /// <summary>
        /// 满减金额
        /// </summary>
        public decimal Off { get; set; }

        #region 收货人相关

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        public string Address { get; set; }

        #endregion
    }
}