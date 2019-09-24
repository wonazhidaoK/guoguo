namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllForShopUserSmallDistrictShopInput
    {
        ///// <summary>
        ///// 业主认证申请Id(getListId)
        ///// </summary>
        //public string ApplicationRecordId { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }
    }
}