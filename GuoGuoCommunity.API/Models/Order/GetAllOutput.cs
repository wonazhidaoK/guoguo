using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllOutputModel> List { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllOutputModel {
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
        /// 状态变更按钮显示标识
        /// </summary>
        public bool IsBtnDisplay { get; set; }

        /// <summary>
        /// 接单按钮显示标识
        /// </summary>
        public bool IsAcceptBtnDisplay { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}