using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadInput
    {
        /// <summary>
        /// Owner:业主人证提交文件;VipOwner:高级认证提交文件;Announcement:公告提交文件
        /// </summary>
       // [Required]
        public string Type { get; set; }
    }
}