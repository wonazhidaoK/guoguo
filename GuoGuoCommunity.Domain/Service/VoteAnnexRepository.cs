using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class VoteAnnexRepository : IVoteAnnexRepository
    {
        public async Task<VoteAnnex> AddAsync(VoteAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.VoteAnnices.Add(new VoteAnnex
                {
                    VoteId = dto.VoteId,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteAnnex>> GetAllAsync(VoteAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteAnnex>> GetListAsync(VoteAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
