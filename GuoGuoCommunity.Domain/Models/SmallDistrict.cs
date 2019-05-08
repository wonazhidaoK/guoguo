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

        ///// <summary>
        ///// 街道办Id
        ///// </summary>
        //[Required]
        //[ForeignKey("SmallDistrict_StreetOffice")]
        //public Guid StreetOfficeId { get; set; }

        //public StreetOffice SmallDistrict_StreetOffice { get; set; }

        /// <summary>
        /// 社区Id
        /// </summary>
        [Required]
        [ForeignKey("Community")]
        public Guid CommunityId { get; set; }

        public Community Community { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public string IsInvalid { get; set; }

        /// <summary>
        /// 是否选举
        /// </summary>
        public bool IsElection { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
