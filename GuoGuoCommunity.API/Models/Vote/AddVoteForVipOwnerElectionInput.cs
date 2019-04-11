using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddVoteForVipOwnerElectionInput
    {
        /// <summary>
        /// 业委会Id
        /// </summary>
        public string VipOwnerId { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// 小区
        /// </summary>
        public string SmallDistrictId { get; set; }

        ///// <summary>
        ///// 人员集合(业委会成员申请记录Id)
        ///// </summary>
        //public List<string> List { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }
    }
}