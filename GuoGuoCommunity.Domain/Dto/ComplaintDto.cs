using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class ComplaintDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 投诉类型Id
        /// </summary>
        public string ComplaintTypeId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }

        /// <summary>
        /// 操作人部门名称
        /// </summary>
        public string OperationDepartmentName { get; set; }

        /// <summary>
        /// 操作人部门值
        /// </summary>
        public string OperationDepartmentValue { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        #region 街道办结构

        /// <summary>
        /// 街道办Id
        /// </summary>
        public string StreetOfficeId { get; set; }

        /// <summary>
        /// 社区Id
        /// </summary>
        public string CommunityId { get; set; }
        
        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        #endregion

        /// <summary>
        /// 状态值
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTimeOffset? EndTime { get; set; }
    }
}
