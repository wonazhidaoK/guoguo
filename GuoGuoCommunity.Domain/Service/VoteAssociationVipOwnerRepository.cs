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
    public class VoteAssociationVipOwnerRepository : IVoteAssociationVipOwnerRepository
    {
        public async Task<VoteAssociationVipOwner> AddAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.VoteAssociationVipOwners.Add(new VoteAssociationVipOwner
                {
                    VipOwnerId = dto.VipOwnerId,
                    VoteId = dto.VoteId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteAssociationVipOwner>> GetAllAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteAssociationVipOwner> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<VoteAssociationVipOwner> GetForVoteIdAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.VoteAssociationVipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                return new VoteAssociationVipOwner();
            }
        }

        public Task<List<VoteAssociationVipOwner>> GetListAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteAssociationVipOwnerDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
