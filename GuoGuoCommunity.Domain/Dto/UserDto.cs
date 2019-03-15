namespace GuoGuoCommunity.Domain.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

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
        public string RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 微信Openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 微信Unionid
        /// </summary>
        public string UnionId { get; set; }

        /// <summary>
        /// 是否是业主
        /// </summary>
        public string IsOwner { get; set; }

        /// <summary>
        /// 是否是业委会成员
        /// </summary>
        public string IsVipOwner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
