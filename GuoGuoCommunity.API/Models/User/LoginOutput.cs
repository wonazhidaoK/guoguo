namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string Region { get; set; }

        ///// <summary>
        ///// 菜单英文名
        ///// </summary>
        //public string Key { get; set; }

        ///// <summary>
        ///// 是否显示
        ///// </summary>
        //public bool IsDisplayed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }

        // <summary>
        // 
        //</summary>
        //public string refresh_token { get; set; }
    }
}