using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class RoleMenuDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RolesId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisplay { get; set; }

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
