using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class VipOwnerCertificationConditionRepository : IVipOwnerCertificationConditionRepository
    {
        public Task<VipOwnerCertificationCondition> AddAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationCondition>> GetAllAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerCertificationCondition> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationCondition>> GetListAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
