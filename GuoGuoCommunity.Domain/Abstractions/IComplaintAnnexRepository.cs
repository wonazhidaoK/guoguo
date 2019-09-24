using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IComplaintAnnexRepository : IIncludeRepository<ComplaintAnnex, ComplaintAnnexDto>
    {
        /// <summary>
        /// 根据投诉跟进id 获取投诉附件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ComplaintAnnex> AddForFollowUpIdAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据投诉Id获取投诉附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ComplaintAnnex> GetByComplaintIdIncludeAsync(string id, CancellationToken token = default);

        /// <summary>
        /// 根据投诉Id集合获取投诉附件集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<ComplaintAnnex>> GetByComplaintIdsAsync(List<string> ids, CancellationToken token = default);

        /// <summary>
        /// 根据投诉跟进Id集合获取投诉附件集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<ComplaintAnnex>> GetByFollowUpIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
