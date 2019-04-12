using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteRecordRepository
    {
        Task<VoteRecord> AddAsync(VoteRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteRecordDto dto, CancellationToken token = default);

        Task<List<VoteRecord>> GetAllAsync(VoteRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteRecordDto dto, CancellationToken token = default);

        Task<VoteRecord> GetAsync(string id, CancellationToken token = default);

        Task<VoteRecord> GetForOwnerCertificationIdAsync(VoteRecordDto dto, CancellationToken token = default);

        Task<List<VoteRecord>> GetListAsync(VoteRecordDto dto, CancellationToken token = default);
    }
}
