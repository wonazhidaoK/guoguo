namespace GuoGuoCommunity.API.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class GetCommunityOutput
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// 社区名称
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
    }
}