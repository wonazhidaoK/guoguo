namespace GuoGuoCommunity.Domain.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        ///// <summary>
        ///// 登陆账号
        ///// </summary>
        //public string Account { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RolesId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
