using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    interface IOwnerCertificationRecordRepository
    {
        Task<OwnerCertificationRecord> AddAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateStatusAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetAllAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<OwnerCertificationRecord> GetAsync(string id, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetListAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetListForOwnerAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);
    }
}
