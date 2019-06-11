using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 付款类型
    /// </summary>
    public class PaymentType
    {
        static PaymentType()
        {
            WeiXin = new PaymentType { Name="微信支付", Value= "WeiXin" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public static PaymentType WeiXin { get; set; }

        public static IEnumerable<PaymentType> GetAll() => new List<PaymentType>() { WeiXin};
    }
}
