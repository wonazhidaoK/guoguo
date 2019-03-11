namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 获取菜单
    /// </summary>
    public class GetAllMenuOutput
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单值
        /// </summary>
        public string Kay { get; set; }

    }
}