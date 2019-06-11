using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 小区
    /// </summary>
    public class SmallDistrict : IEntitity, ICommunity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Required]
        public string Region { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 社区Id
        /// </summary>
        [Required]
        [ForeignKey("Community")]
        public Guid CommunityId { get; set; }

        public Community Community { get; set; }

        /// <summary>
        /// 物业公司Id
        /// </summary>
        [ForeignKey("PropertyCompany")]
        public Guid? PropertyCompanyId { get; set; }

        public PropertyCompany PropertyCompany { get; set; }

        #region 物业账户扩展信息

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        #endregion

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
