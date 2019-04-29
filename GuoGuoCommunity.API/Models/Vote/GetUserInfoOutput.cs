using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetUserInfoOutput
    {
        /// <summary>
        /// 职能名称
        /// </summary>
        public string StructureName { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 高级认证附件
        /// </summary>
        public List<AnnexModel> List { get; set; }

        #region 业主认证信息
        
        ///// <summary>
        ///// 社区名称
        ///// </summary>
        //public string CommunityName { get; set; }

        ///// <summary>
        ///// 小区名称
        ///// </summary>
        //public string SmallDistrictName { get; set; }

        ///// <summary>
        ///// 楼宇名称
        ///// </summary>
        //public string BuildingName { get; set; }

        ///// <summary>
        ///// 楼宇单元名称
        ///// </summary>
        //public string BuildingUnitName { get; set; }

        ///// <summary>
        ///// 业户名称
        ///// </summary>
        //public string IndustryName { get; set; }

        /// <summary>
        /// 地址(省市区+小区+楼宇+单元+业户)
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region 省市区信息
        ///// <summary>
        ///// 省
        ///// </summary>
        //public string State { get; set; }

        ///// <summary>
        ///// 市
        ///// </summary>
        //public string City { get; set; }

        ///// <summary>
        ///// 区
        ///// </summary>
        //public string Region { get; set; }

        #endregion

        #region 业主基本信息

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
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        #endregion

        #region 微信用户信息

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Headimgurl { get; set; }
        
        #endregion
    }
}