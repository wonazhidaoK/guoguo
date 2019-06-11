using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 微信Openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 微信Unionid
        /// </summary>
        public string UnionId { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }

        #region 省市区

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

        #endregion

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
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }

        public string ShopId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public class UserPageDto
    {
        public List<User> List { get; set; }

        public int Count { get; set; }
    }
}
