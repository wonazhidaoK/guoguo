using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteRecordRepository : IIncludeRepository<VoteRecord, VoteRecordDto>
    {
        /// <summary>
        /// 根据业主认证Id查询投票详情
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VoteRecord> GetForOwnerCertificationIdAsync(VoteRecordDto dto, CancellationToken token = default);
    }
}
