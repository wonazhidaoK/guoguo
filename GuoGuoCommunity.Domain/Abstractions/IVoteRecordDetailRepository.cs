using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public  interface IVoteRecordDetailRepository
    {
        Task<VoteRecordDetail> AddAsync(VoteRecordDetailDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteRecordDetailDto dto, CancellationToken token = default);

        Task<List<VoteRecordDetail>> GetAllAsync(VoteRecordDetailDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteRecordDetailDto dto, CancellationToken token = default);

        Task<VoteRecordDetail> GetAsync(string id, CancellationToken token = default);

        Task<List<VoteRecordDetail>> GetListAsync(VoteRecordDetailDto dto, CancellationToken token = default);
    }
}
