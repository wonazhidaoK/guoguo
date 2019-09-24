using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IComplaintFollowUpRepository : IIncludeRepository<ComplaintFollowUp, ComplaintFollowUpDto>
    {
        /// <summary>
        /// 根据投诉Id获取跟进详情集合
        /// </summary>
        /// <param name="complaintId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<ComplaintFollowUp>> GetListForComplaintIdAsync(string complaintId, CancellationToken token = default);
    }
}
