using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetListStreetOfficeAnnouncementOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GetListStreetOfficeAnnouncementModelOutput> List { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetListStreetOfficeAnnouncementModelOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

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
        /// 
        /// </summary>
        public DateTimeOffset ReleaseTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 附件url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 小区范围
        /// </summary>
        public List<SmallDistrictModel> SmallDistrict { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SmallDistrictModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
    }
}