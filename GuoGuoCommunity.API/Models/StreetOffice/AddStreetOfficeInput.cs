using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddStreetOfficeInput
    {
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
        /// 街道办名称
        /// </summary>
        public string Name { get; set; }
    }
}