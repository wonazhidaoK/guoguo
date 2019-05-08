using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 楼宇
    /// </summary>
    public class Building : IEntitity,ISmallDistrict
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        /// <summary>
        ///楼宇名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        [Required]
        [ForeignKey("SmallDistrict")]
        public Guid SmallDistrictId { get; set; }

        ///// <summary>
        ///// 小区名称
        ///// </summary>
        //[Required]
        //public string SmallDistrictName { get; set; }
        public SmallDistrict  SmallDistrict { get; set; }
        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
