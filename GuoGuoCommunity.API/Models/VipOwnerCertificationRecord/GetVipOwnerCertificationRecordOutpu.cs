using System;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetVipOwnerCertificationRecordOutpu
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        ///// <summary>
        ///// 业委会Id
        ///// </summary>
        //public string VipOwnerId { get; set; }

        /// <summary>
        /// 业委会名称
        /// </summary>
        public string VipOwnerName { get; set; }

        ///// <summary>
        ///// 业委会职能Id
        ///// </summary>
        //public string VipOwnerStructureId { get; set; }

        /// <summary>
        /// 业委会职能名称
        /// </summary>
        public string VipOwnerStructureName { get; set; }

        ///// <summary>
        ///// 用户Id
        ///// </summary>
        //public string UserId { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public bool IsInvalid { get; set; }

        ///// <summary>
        ///// 业主认证Id
        ///// </summary>
        //public string OwnerCertificationId { get; set; }


        /// <summary>
        /// 业主名称
        /// </summary>
        public string OwnerCertificationName { get; set; }

        ///// <summary>
        ///// 投票id
        ///// </summary>
        //public string VoteId { get; set; }

        /// <summary>
        /// 认证时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}