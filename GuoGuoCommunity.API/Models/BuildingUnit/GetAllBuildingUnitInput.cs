using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllBuildingUnitInput
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

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageSize { get; set; }
    }
}