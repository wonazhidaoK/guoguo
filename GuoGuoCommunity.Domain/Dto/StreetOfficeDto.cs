using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public  class StreetOfficeDto
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
        /// 街道办名称
        /// </summary>
      
        public string Name { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
