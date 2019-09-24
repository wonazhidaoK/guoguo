namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

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
        /// 是否有站内信未读
        /// </summary>
        public bool IsHaveUnRead { get; set; }

        ///// <summary>
        ///// 菜单英文名
        ///// </summary>
        //public string Key { get; set; }

        ///// <summary>
        ///// 是否显示
        ///// </summary>
        //public bool IsDisplayed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }

        // <summary>
        // 
        //</summary>
        //public string refresh_token { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 商店Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        /// 物业公司Id
        /// </summary>
        public string PropertyCompanyId { get; set; }
        /// <summary>
        /// 活动开启标记 ,0未开启活动，1开启满减活动，1,2开启满减和某其他活动 一次类推
        /// </summary>
        public string ActivitySign { get; set; }
    }
}