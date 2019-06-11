namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateShopInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 商超类别值
        /// </summary>
        public string MerchantCategoryValue { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        ///// <summary>
        ///// 资质图片Id
        ///// </summary>
        //public string QualificationImageId { get; set; }

        ///// <summary>
        ///// Logo图片Id
        ///// </summary>
        //public string LogoImageId { get; set; }

        /// <summary>
        /// 资质图片Id
        /// </summary>
        public string QualificationImageUrl { get; set; }

        /// <summary>
        /// Logo图片Id
        /// </summary>
        public string LogoImageUrl { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }
    }
}