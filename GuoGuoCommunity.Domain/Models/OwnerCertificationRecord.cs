using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 业主认证记录
    /// </summary>
    public class OwnerCertificationRecord : IDeleted, ILastOperation, ICreateOperation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        #region 基础信息

        /// <summary>
        /// 街道办Id
        /// </summary>
        public string StreetOfficeId { get; set; }

        /// <summary>
        /// 街道办名称
        /// </summary>
        public string StreetOfficeName { get; set; }

        /// <summary>
        /// 社区Id
        /// </summary>
        public string CommunityId { get; set; }

        /// <summary>
        /// 社区名称
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string SmallDistrictName { get; set; }

        /// <summary>
        /// 楼宇Id
        /// </summary> 
        public string BuildingId { get; set; }

        /// <summary>
        /// 楼宇名称
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// 楼宇单元Id
        /// </summary>
        public string BuildingUnitId { get; set; }

        /// <summary>
        /// 楼宇单元名称
        /// </summary>
        public string BuildingUnitName { get; set; }

        /// <summary>
        /// 业户Id
        /// </summary>
        public string IndustryId { get; set; }

        /// <summary>
        /// 业户名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// 业主名称
        /// </summary>
        public string OwnerName { get; set; }

        #endregion

        /// <summary>
        /// 认证时间
        /// </summary>
        public string CertificationTime { get; set; }

        /// <summary>
        /// 认证结果
        /// </summary>
        public string CertificationResult { get; set; }

        /// <summary>
        /// 认证状态1.认证中2.认证通过3.认证失败
        /// </summary>
        public string CertificationStatusName { get; set; }

        /// <summary>
        /// 认证状态值
        /// </summary>
        public string CertificationStatusValue { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsInvalid { get; set; }

        public string LastOperationUserId { get; set; }
        public DateTimeOffset? LastOperationTime { get; set; }
        public string CreateOperationUserId { get; set; }
        public DateTimeOffset? CreateOperationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
