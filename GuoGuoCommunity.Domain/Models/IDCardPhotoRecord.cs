using GuoGuoCommunity.Domain.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuoGuoCommunity.Domain.Models
{
    /// <summary>
    /// 身份证照片读取记录
    /// </summary>
    public class IDCardPhotoRecord : ICreateOperation
    {
        /*
         * 1.业务id
         * 2.操作时间
         * 3.照片路径 +Base64存储
         * 4.读取数据
         */
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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


        public string CreateOperationUserId { get; set; }


        public DateTimeOffset? CreateOperationTime { get; set; }
    }
}
