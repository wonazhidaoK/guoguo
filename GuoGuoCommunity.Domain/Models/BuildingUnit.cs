using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 楼宇单元信息
    /// </summary>
    public class BuildingUnit : IEntitity,IBuilding
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 单元名称
        /// </summary>
        [Required]
        public string  UnitName { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int NumberOfLayers { get; set; }

        /// <summary>
        /// 楼宇Id
        /// </summary>
        [Required]
        [ForeignKey("Building")]
        public Guid BuildingId { get; set; }

        public Building Building { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime  { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
