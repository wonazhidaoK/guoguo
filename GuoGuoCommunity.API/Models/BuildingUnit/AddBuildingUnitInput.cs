namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddBuildingUnitInput
    {
        /// <summary>
        /// 单元名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public string NumberOfLayers { get; set; }

        /// <summary>
        /// 楼宇Id
        /// </summary>
        public string BuildingId { get; set; }
    }
}