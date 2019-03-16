using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class VipOwnerCertificationRecordRepository : IVipOwnerCertificationRecordRepository
    {
        public Task<VipOwnerCertificationRecord> AddAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<VipOwnerCertificationRecord>> GetAllAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<VipOwnerCertificationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<VipOwnerCertificationRecord>> GetListAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
