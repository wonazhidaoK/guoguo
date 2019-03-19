namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateStreetOfficeUserInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }
    }
}