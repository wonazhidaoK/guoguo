namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WXLoginOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 是否是业主
        /// </summary>
        public bool IsOwner { get; set; }

        ///// <summary>
        ///// 是否是业委会成员
        ///// </summary>
        //public bool IsVipOwner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Headimgurl { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsSubscription { get; set; }
    }
}