namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddOwnerCertificationRecordInput
    {
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
        /// 社区Id
        /// </summary>
        public string CommunityId { get; set; }
        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 楼宇Id
        /// </summary> 
        public string BuildingId { get; set; }

        /// <summary>
        /// 楼宇单元Id
        /// </summary>
        public string BuildingUnitId { get; set; }

        /// <summary>
        /// 业户Id
        /// </summary>
        public string IndustryId { get; set; }

        #endregion
    }
}