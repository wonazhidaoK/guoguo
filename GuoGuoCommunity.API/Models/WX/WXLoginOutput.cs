using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WXLoginOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 是否是业主
        /// </summary>
        public string IsOwner { get; set; }

        /// <summary>
        /// 是否是业委会成员
        /// </summary>
        public string IsVipOwner { get; set; }
    }
}