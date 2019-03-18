using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddVipOwnerCertificationRecordInput
    {
        /// <summary>
        /// 职能Id
        /// </summary>
        public string StructureId { get; set; }


        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Model>  Models { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Model
    {
        /// <summary>
        /// 条件Id
        /// </summary>
        public string ConditionId { get; set; }

        /// <summary>
        /// 上传Id
        /// </summary>
        public string UploadId { get; set; }
    }
}