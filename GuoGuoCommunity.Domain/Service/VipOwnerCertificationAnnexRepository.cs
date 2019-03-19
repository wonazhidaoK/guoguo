using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class VipOwnerCertificationAnnexRepository : IVipOwnerCertificationAnnexRepository
    {
        public async Task<VipOwnerCertificationAnnex> AddAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.VipOwnerCertificationAnnices.Add(new VipOwnerCertificationAnnex
                {
                    ApplicationRecordId = dto.ApplicationRecordId,
                    CertificationConditionId = dto.CertificationConditionId,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
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

        public async Task<List<VipOwnerCertificationAnnex>> GetListAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationAnnices.Where(x =>  x.ApplicationRecordId == dto.ApplicationRecordId).ToListAsync(token);
            }
        }

        public Task UpdateAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
