using System;
using System.Collections.Generic;

namespace GuoGuoCommunity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetVoteOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

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
        public string SmallDistrictArray { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门值
        /// </summary>
        public string DepartmentValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 问题集合
        /// </summary>
        public List<GetVoteQuestionModel> List { get; set; }
    }
    /// <summary>
    /// 投票问题
    /// </summary>
    public class GetVoteQuestionModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 选项方式0:单选1:多选
        /// </summary>
        public string OptionMode { get; set; }

        /// <summary>
        /// 选项集合
        /// </summary>
        public List<GetVoteQuestionOptionModel> List { get; set; }
    }

    /// <summary>
    /// 投票问题选项
    /// </summary>
    public class GetVoteQuestionOptionModel
    {
        /// <summary>
        /// 选项描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 票数
        /// </summary>
        public int Votes { get; set; }
    }
}