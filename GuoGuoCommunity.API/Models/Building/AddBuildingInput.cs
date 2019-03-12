namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddBuildingInput
    {
        /// <summary>
        ///楼宇名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string SmallDistrictName { get; set; }
    }
}