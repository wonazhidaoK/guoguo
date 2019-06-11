using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderStatus
    {
        static OrderStatus()
        {
            WaitingAccept = new OrderStatus { Name = "待接单", Value = "WaitingAccept" };
            WaitingSend = new OrderStatus { Name = "待配送", Value = "WaitingSend" };
            WaitingTake = new OrderStatus { Name = "待取货", Value = "WaitingTake" };
            WaitingReceive = new OrderStatus { Name = "待收货", Value = "WaitingReceive" };
            WaitingAppraise = new OrderStatus { Name = "待评价", Value = "WaitingAppraise" };
            Closed = new OrderStatus { Name = "已关闭", Value = "Closed" };
            Finish = new OrderStatus { Name = "完成", Value = "Finish" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 待接单
        /// </summary>
        public static OrderStatus WaitingAccept { get; set; }

        /// <summary>
        /// 待配送
        /// </summary>
        public static OrderStatus WaitingSend { get; set; }

        /// <summary>
        /// 待取货
        /// </summary>
        public static OrderStatus WaitingTake { get; set; }

        /// <summary>
        /// 待收货
        /// </summary>
        public static OrderStatus WaitingReceive { get; set; }

        /// <summary>
        /// 待评价
        /// </summary>
        public static OrderStatus WaitingAppraise { get; set; }

        /// <summary>
        /// 已关闭
        /// </summary>
        public static OrderStatus Closed { get; set; }

        /// <summary>
        /// 完成
        /// </summary>
        public static OrderStatus Finish { get; set; }

        public static IEnumerable<OrderStatus> GetAll() => new List<OrderStatus>() { WaitingAccept, WaitingSend, WaitingTake, WaitingReceive, WaitingAppraise, Finish };
    }
}
