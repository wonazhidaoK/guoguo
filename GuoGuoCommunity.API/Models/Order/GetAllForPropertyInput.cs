namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllOrderForPropertyInput
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 订单状态值
        /// </summary>
        public string OrderStatusValue { get; set; }

        /// <summary>
        /// 商店Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }
    }
}