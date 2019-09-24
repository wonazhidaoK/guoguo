using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteResultRecordRepository : IIncludeRepository<VoteResultRecord, VoteResultRecordDto>
    {
        Task<VoteResultRecord> AddVipOwnerElectionAsync(VoteResultRecordDto dto, CancellationToken token = default);

        Task<List<VoteResultRecord>> GetAllAsync(AnnouncementAnnexDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据投票Id查询投票记录(弃用)
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VoteResultRecord> GetForVoteIdAsync(string voteId, CancellationToken token = default);

        Task<VoteResultRecord> GetForVoteQuestionIdAsync(VoteResultRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据投票Id查询投票记录集合
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VoteResultRecord>> GetListForVoteIdAsync(string voteId, CancellationToken token = default);
    }
}
