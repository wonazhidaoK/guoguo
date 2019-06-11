using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllShopUserOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetAllShopUserOutputModel> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllShopUserOutputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}