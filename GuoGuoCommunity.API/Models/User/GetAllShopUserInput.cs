using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAllShopUserInput
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 商家Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PageSize { get; set; }
    }
}