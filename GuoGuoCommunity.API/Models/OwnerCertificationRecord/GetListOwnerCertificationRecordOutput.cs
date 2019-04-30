using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetListOwnerCertificationRecordOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetOwnerCertificationRecordOutput> List { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetOwnerCertificationRecordOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

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
        /// 小区电话
        /// </summary>
        public string SmallDistrictPhoneNumber { get; set; }

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

        #region 业主信息

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        #endregion


        #region 业户信息

        /// <summary>
        /// 面积
        /// </summary>
        public string Acreage { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int? NumberOfLayers { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        public string Oriented { get; set; }

        #endregion

    }
}