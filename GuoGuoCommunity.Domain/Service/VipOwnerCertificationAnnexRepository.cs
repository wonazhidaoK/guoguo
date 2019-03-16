using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class VipOwnerCertificationAnnexRepository : IVipOwnerCertificationAnnexRepository
    {
        public Task<VipOwnerCertificationAnnex> AddAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationAnnex>> GetAllAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationAnnex>> GetListAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
