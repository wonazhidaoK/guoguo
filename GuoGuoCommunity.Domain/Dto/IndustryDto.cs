using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public  class IndustryDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 业户门号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int NumberOfLayers { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public string Acreage { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        public string Oriented { get; set; }

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
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作人所属小区Id
        /// </summary>
        public string OperationUserSmallDistrictId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
