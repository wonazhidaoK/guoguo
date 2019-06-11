using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public  class SmallDistrictDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

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
        /// 小区电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        /// <summary>
        /// 物业公司Id
        /// </summary>
        public string PropertyCompanyId { get; set; }
    }
}
