namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetListStreetOfficeInput
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
    }
}