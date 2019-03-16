using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class VipOwnerApplicationRecordRepository : IVipOwnerApplicationRecordRepository
    {
        public Task<VipOwnerApplicationRecord> AddAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerApplicationRecord>> GetAllAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerApplicationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerApplicationRecord>> GetListAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
