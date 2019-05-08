using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 社区
    /// </summary>
    public class Community : IEntitity, IStreetOffice
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
        /// 社区名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 街道办Id
        /// </summary>
        [Required]
        [ForeignKey("StreetOffice")]
        public Guid StreetOfficeId { get; set; }

        public StreetOffice  StreetOffice { get; set; }
        ///// <summary>
        ///// 街道办名称
        ///// </summary>
        //[Required]
        //public string StreetOfficeName { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
