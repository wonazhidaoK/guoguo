using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IComplaintRepository : IIncludeRepository<Complaint, ComplaintDto>
    {
        /// <summary>
        /// 更改投诉 申述状态
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateForAppealAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 更改投诉状态为已完结
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateForFinishedAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 街道办更改投诉状态为已完成
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 提供给业委会 投诉列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Complaint>> GetAllForVipOwnerAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 提供给物业 投诉列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Complaint>> GetAllForPropertyIncludeAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 提供给街道办 投诉列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Complaint>> GetAllForStreetOfficeIncludeAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 关闭投诉  投诉状态为已完成
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ClosedAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 业业委会查看投诉  投诉状态为处理中
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ViewForVipOwnerAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 物业查看投诉 投诉状态为处理中
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ViewForPropertyAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 街道办查看投诉 投诉状态为处理中
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ViewForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default);

        /// <summary>
        /// 投诉无效  投诉状态为已完成
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task InvalidAsync(ComplaintDto dto, CancellationToken token = default);
    }
}
