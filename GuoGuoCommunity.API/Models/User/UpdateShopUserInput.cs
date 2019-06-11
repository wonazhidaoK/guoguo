namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateShopUserInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
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

        ///// <summary>
        ///// 地址
        ///// </summary>
        //public string Address { get; set; }

        ///// <summary>
        ///// 商超类别值
        ///// </summary>
        //public string MerchantCategoryValue { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }

        ///// <summary>
        ///// 描述
        ///// </summary>
        //public string Description { get; set; }

        ///// <summary>
        ///// 资质图片Id
        ///// </summary>
        //public string QualificationImageId { get; set; }

        ///// <summary>
        ///// Logo图片Id
        ///// </summary>
        //public string LogoImageId { get; set; }
    }
}