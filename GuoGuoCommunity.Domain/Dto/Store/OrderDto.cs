using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Dto
{
    public class OrderDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string OrderStatusName { get; set; }

        /// <summary>
        /// 订单状态值
        /// </summary>
        public string OrderStatusValue { get; set; }

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

        #region 商户信息

        public string ShopId { get; set; }

        /// <summary>
        /// 配送费 
        /// </summary>
        public string Postage { get; set; }

        #endregion

        #region 商品信息

        /// <summary>
        /// 订单商品总件数
        /// </summary>
        public int ShopCommodityCount { get; set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal ShopCommodityPrice { get; set; }

        #endregion

        #region 付款信息

        /// <summary>
        /// 付款状态名称
        /// </summary>
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 付款状态值
        /// </summary>
        public string PaymentStatusValue { get; set; }

        /// <summary>
        /// 付款类型名称
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 付款类型值
        /// </summary>
        public string PaymentTypeValue { get; set; }

        #endregion

        #region 下单人信息

        /// <summary>
        /// 用户认证ID（外键）
        /// </summary>
        public string OwnerCertificationRecordId { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        #endregion

        #region 收货人地址信息

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

    public class OrderItemModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string OrderStatusName { get; set; }

        /// <summary>
        /// 订单状态值
        /// </summary>
        public string OrderStatusValue { get; set; }

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

        #region 商户信息

        public string ShopId { get; set; }

        /// <summary>
        /// 配送费 
        /// </summary>
        public string Postage { get; set; }

        #endregion

        #region 商品信息

        /// <summary>
        /// 订单商品总件数
        /// </summary>
        public int ShopCommodityCount { get; set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal ShopCommodityPrice { get; set; }

        #endregion

        #region 付款信息

        /// <summary>
        /// 付款状态名称
        /// </summary>
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 付款状态值
        /// </summary>
        public string PaymentStatusValue { get; set; }

        /// <summary>
        /// 付款类型名称
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 付款类型值
        /// </summary>
        public string PaymentTypeValue { get; set; }

        #endregion

        #region 下单人信息

        /// <summary>
        /// 用户认证ID（外键）
        /// </summary>
        public string OwnerCertificationRecordId { get; set; }

        #endregion

        #region 收货人地址信息

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

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        public List<OrderItem> List { get; set; }

    }
    public class OrderForPageDto
    {
        public List<Order> List { get; set; }

        public int Count { get; set; }
    }
}
