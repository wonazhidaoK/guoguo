using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 订单推送
    /// </summary>
    public class OrderPushModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 应付款金额
        /// </summary>
        public decimal PaymentPrice { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Type { get; set; }
    }
}