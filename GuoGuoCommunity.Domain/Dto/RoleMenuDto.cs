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
    }
}
