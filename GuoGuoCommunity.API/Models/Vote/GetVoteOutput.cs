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
        public string Deadline { get; set; }

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

        /// <summary>
        /// 应参与人数
        /// </summary>
        public string ShouldParticipateCount { get; set; }

        /// <summary>
        /// 当前人投票状态
        /// </summary>
        public bool IsVoted { get; set; }

        /// <summary>
        /// 投票状态
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// 投票类型值
        /// </summary>
        public string VoteTypeValue { get; set; }

        /// <summary>
        /// 投票类型名称
        /// </summary>
        public string VoteTypeName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 小区范围名称
        /// </summary>
        public string SmallDistrictArrayName { get; set; }

        /// <summary>
        /// 投票反馈
        /// </summary>
        public string Feedback { get; set; }
    }
    /// <summary>
    /// 投票问题
    /// </summary>
    public class GetVoteQuestionModel
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
        /// 选项方式0:单选1:多选
        /// </summary>
        public string OptionMode { get; set; }

        /// <summary>
        /// 选择数
        /// </summary>
        public int ElectionNumber { get; set; }

        /// <summary>
        /// 选项集合
        /// </summary>
        public List<GetVoteQuestionOptionModel> List { get; set; }

        /// <summary>
        /// 投票结果名称
        /// </summary>
        public string VoteResultName { get; set; }

        /// <summary>
        /// 投票结果名称值
        /// </summary>
        public string VoteResultValue { get; set; }
    }

    /// <summary>
    /// 投票问题选项
    /// </summary>
    public class GetVoteQuestionOptionModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 选项描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 票数
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Headimgurl { get; set; }
    }
}