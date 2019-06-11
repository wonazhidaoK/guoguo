namespace GuoGuoCommunity.API.Models
{
    public class GetListForShopUserInput
    {
        /// <summary>
        /// 商品类别
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 业主申请记录Id
        /// </summary>
        public string ApplicationRecordId { get; set; }
    }
}