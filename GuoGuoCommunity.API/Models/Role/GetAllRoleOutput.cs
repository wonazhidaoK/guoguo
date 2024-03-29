﻿using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllRoleOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetRoleOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetRoleOutput
    {
        /// <summary>
        ///  唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
    }
}