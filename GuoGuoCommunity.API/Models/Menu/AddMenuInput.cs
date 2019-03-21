namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 添加菜单入参
    /// </summary>
    public class AddMenuInput
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单值
        /// </summary>
        public string Key { get; set; }
    }
}