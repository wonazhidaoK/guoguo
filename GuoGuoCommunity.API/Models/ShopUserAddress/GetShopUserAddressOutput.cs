namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetShopUserAddressOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 业户Id
        /// </summary>
        public string IndustryId { get; set; }

        /// <summary>
        /// 业户名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 楼宇Id
        /// </summary>
        public string BuildingId { get; set; }

        /// <summary>
        /// 楼宇名称
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// 楼宇单元Id
        /// </summary>
        public string BuildingUnitId { get; set; }

        /// <summary>
        /// 楼宇单元名称
        /// </summary>
        public string BuildingUnitName { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int NumberOfLayers { get; set; }

        ///// <summary>
        ///// 地址
        ///// </summary>
        //public string Address { get; set; }
    }
}