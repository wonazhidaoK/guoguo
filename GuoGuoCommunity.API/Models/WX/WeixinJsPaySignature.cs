namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 返回小程序端模型
    /// </summary>
    public class WeixinJsPaySignature
    {
        /// <summary>
        /// AppID
        /// </summary>
        public object AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NonceStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Package { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PaySign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignType { get; set; }
    }
}