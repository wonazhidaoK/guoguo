using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuoGuoCommunity.API.Models
{
    public class AddPropertyAnnouncementInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }
    }
}