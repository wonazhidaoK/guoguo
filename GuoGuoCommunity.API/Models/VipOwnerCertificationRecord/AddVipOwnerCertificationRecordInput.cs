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
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

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
        /// 附件内容
        /// </summary>
        public string AnnexContent { get; set; }
    }
}