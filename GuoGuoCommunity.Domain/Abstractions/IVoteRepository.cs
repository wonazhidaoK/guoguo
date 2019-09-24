using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteRepository : IIncludeRepository<Vote, VoteDto>
    {
        /// <summary>
        /// 业委会新增投票
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Vote> AddForVipOwnerAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 街道办添加投票
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Vote> AddForStreetOfficeAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 更改投票状态为关闭
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateForClosedAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 更改投票结果计算方式
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateCalculationMethodAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 街道办查询投票集合
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Vote>> GetAllForStreetOfficeAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 物业查询投票集合
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Vote>> GetAllForPropertyAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 业委会查询投票集合
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Vote>> GetAllForVipOwnerAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 业主查询投票集合
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Vote>> GetAllForOwnerAsync(VoteDto dto, CancellationToken token = default);

        /// <summary>
        /// 查询应结束投票
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Vote>> GetDeadListAsync(VoteDto dto, CancellationToken token = default);
    }
}
