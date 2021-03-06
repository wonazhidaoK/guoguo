﻿using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddVoteForVipOwnerInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string Deadline { get; set; }

        /// <summary>
        /// 投票类型 RecallProperty罢免物业，Ordinary普通投票
        /// </summary>
        public string VoteTypeValue { get; set; }

        /// <summary>
        /// 问题集合
        /// </summary>
        public List<AddVoteQuestionModel> List { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }

        /// <summary>
        /// 业主认证Id
        /// </summary>
        public string OwnerCertificationId { get; set; }
    }
}