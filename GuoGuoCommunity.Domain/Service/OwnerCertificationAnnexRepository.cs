using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class OwnerCertificationAnnexRepository : IOwnerCertificationAnnexRepository
    {
        public async Task<OwnerCertificationAnnex> AddAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.OwnerCertificationAnnices.Add(new OwnerCertificationAnnex
                {
                    ApplicationRecordId = dto.ApplicationRecordId,
                    OwnerCertificationAnnexTypeValue = dto.OwnerCertificationAnnexTypeValue,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OwnerCertificationAnnex>> GetAllAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<OwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OwnerCertificationAnnex>> GetListAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
