using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddVoteForStreetOfficeInput
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
        public DateTimeOffset Deadline { get; set; }

        /// <summary>
        /// 小区范围
        /// </summary>
        public string SmallDistrict { get; set; }

        /// <summary>
        /// 问题集合
        /// </summary>
        public List<AddVoteQuestionModel> List { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public string AnnexId { get; set; }
    }

    /// <summary>
    /// 投票问题
    /// </summary>
    public class AddVoteQuestionModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        ///// <summary>
        ///// 选项方式0:单选1:多选
        ///// </summary>
        //public string OptionMode { get; set; }

        /// <summary>
        /// 选项集合
        /// </summary>
        public List<AddVoteQuestionOptionModel>  List { get; set; }
    }

    /// <summary>
    /// 投票问题选项
    /// </summary>
    public class AddVoteQuestionOptionModel
    {
        /// <summary>
        /// 选项描述
        /// </summary>
        public string Describe { get; set; }
    }
}