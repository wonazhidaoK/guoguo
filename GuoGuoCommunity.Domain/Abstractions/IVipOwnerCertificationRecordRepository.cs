using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    interface IVipOwnerCertificationRecordRepository
    {
        Task<VipOwnerCertificationRecord> AddAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerCertificationRecord>> GetAllAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<VipOwnerCertificationRecord> GetAsync(string id, CancellationToken token = default);

        Task<List<VipOwnerCertificationRecord>> GetListAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);
    }
}
