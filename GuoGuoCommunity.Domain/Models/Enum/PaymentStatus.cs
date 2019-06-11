using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models
{
    public class PaymentStatus
    {
        static PaymentStatus()
        {
            Paid = new PaymentStatus { Value = "Paid", Name = "已付" };
            Unpaid = new PaymentStatus { Value = "Unpaid", Name = "未付" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 已付
        /// </summary>
        public static PaymentStatus Paid { get; set; }

        /// <summary>
        /// 未付
        /// </summary>
        public static PaymentStatus Unpaid { get; set; }

        public static IEnumerable<PaymentStatus> GetAll() => new List<PaymentStatus>() { Paid, Unpaid };

    }
}
