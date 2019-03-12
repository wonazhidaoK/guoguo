using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class BuildingDto
    {
        public string Id { get; set; }

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

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
