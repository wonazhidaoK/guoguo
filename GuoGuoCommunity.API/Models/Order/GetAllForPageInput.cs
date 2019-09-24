namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForPageInput
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatusValue { get; set; }

        ///// <summary>
        ///// 业主认证申请Id(getListId)
        ///// </summary>
        //public string ApplicationRecordId { get; set; }

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