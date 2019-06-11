using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 商户信息
    /// </summary>
    public class Shop : IEntitity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商家手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

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
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }
    }
}
