namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 登陆名称(admin)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码(Pwd)
        /// </summary>
        public string Pwd { get; set; }
    }
}