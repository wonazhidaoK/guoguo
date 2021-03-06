﻿using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业户信息
    /// </summary>
    public class Industry : IEntitity, IBuildingUnit
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 业户门号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int NumberOfLayers { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public string Acreage { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        public string Oriented { get; set; }

        /// <summary>
        /// 楼宇单元Id
        /// </summary>
        [Required]
        [ForeignKey("BuildingUnit")]
        public Guid BuildingUnitId { get; set; }

        public BuildingUnit BuildingUnit { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        public string LastOperationUserId { get; set; }

        public DateTimeOffset? LastOperationTime { get; set; }

        public string CreateOperationUserId { get; set; }

        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
