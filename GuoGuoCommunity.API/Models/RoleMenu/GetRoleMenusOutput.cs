using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRoleMenusOutput
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单英文名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplayed { get; set; }
    }
}