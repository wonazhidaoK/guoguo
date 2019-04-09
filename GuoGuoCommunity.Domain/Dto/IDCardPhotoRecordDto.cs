using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class IDCardPhotoRecordDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 申请Id(业务Id)
        /// </summary>
        public string ApplicationRecordId { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string OwnerCertificationAnnexId { get; set; }

        /// <summary>
        /// 图片存储
        /// </summary>
        public string PhotoBase64 { get; set; }

        /// <summary>
        /// 阿里云调用返回数据
        /// </summary>
        public string Message { get; set; }

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
