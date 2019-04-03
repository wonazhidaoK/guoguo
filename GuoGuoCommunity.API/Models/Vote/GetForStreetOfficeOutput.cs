using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetForStreetOfficeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 小区范围
        /// </summary>
        public string SmallDistrictArray { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// 是否是创建人
        /// </summary>
        public bool IsCreateUser { get; set; }

        /// <summary>
        /// 投票是否进行中
        /// </summary>
        public bool IsProcessing { get; set; }
    }
}