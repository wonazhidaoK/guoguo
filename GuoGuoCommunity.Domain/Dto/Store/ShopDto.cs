using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class ShopDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商超类别名称
        /// </summary>
        public string MerchantCategoryName { get; set; }

        /// <summary>
        /// 商超类别值
        /// </summary>
        public string MerchantCategoryValue { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 资质图片Url 
        /// </summary>
        public string QualificationImageUrl { get; set; }

        /// <summary>
        /// Logo图片Url
        /// </summary>
        public string LogoImageUrl { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }
    }
}
