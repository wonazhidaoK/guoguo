using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : IEntitity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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

        [Required]
        [ForeignKey("Shop")]
        public Guid ShopId { get; set; }

        public Shop Shop { get; set; }

        /// <summary>
        /// 店铺邮费
        /// </summary>
        public decimal Postage { get; set; }

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

        /// <summary>
        /// 应付款金额
        /// </summary>
        public decimal PaymentPrice { get; set; }

        #endregion

        #region 下单人信息

        /// <summary>
        /// 用户认证ID（外键）
        /// </summary>
        [Required]
        [ForeignKey("OwnerCertificationRecord")]
        public Guid OwnerCertificationRecordId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OwnerCertificationRecord OwnerCertificationRecord { get; set; }

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

        #endregion

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }
    }
}
