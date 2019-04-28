using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetForHistoryVipOwnerApplicationRecordOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 申请人Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职能名称
        /// </summary>
        public string StructureName { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsAdopt { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string SmallDistrictName { get; set; }

        /// <summary>
        ///  业委会名称
        /// </summary>
        public string VipOwnerName { get; set; }

        /// <summary>
        /// 是否当选
        /// </summary>
        public bool IsElected { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}