using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
   public interface IVipOwnerApplicationRecordRepository
    {
        Task<VipOwnerApplicationRecord> AddAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetAllAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetAllForSmallDistrictIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<VipOwnerApplicationRecord> GetAsync(string id, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetListAsync(List<string> dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetListAsync(string userId, CancellationToken token = default);

        Task<bool> IsPresenceforUserId(string userId,CancellationToken token=default);

        Task Adopt(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetListAdoptAsync(List<string> dto, CancellationToken token = default);

        Task UpdateVoteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);
    }
}
