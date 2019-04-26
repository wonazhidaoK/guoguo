using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetVipOwnerApplicationRecordOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 申请人Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职能Id
        /// </summary>
        public string StructureId { get; set; }

        /// <summary>
        /// 职能名称
        /// </summary>
        public string StructureName { get; set; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsAdopt { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public string SmallDistrictId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string SmallDistrictName { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public List<AnnexModel> AnnexModels { get; set; }
    }

    /// <summary>
    /// 附件模型
    /// </summary>
    public class AnnexModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 申请条件Id
        /// </summary>
        public string CertificationConditionId { get; set; }

        /// <summary>
        /// 申请条件名称
        /// </summary>
        public string CertificationConditionName { get; set; }
    }
}