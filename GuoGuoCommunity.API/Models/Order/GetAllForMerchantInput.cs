namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForMerchantInput
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
        /// 
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }
    }
}