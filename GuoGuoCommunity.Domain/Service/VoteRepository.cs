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
    public class VoteRepository : IVoteRepository
    {
        public async Task<Vote> AddAsync(VoteDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId  && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                if (!Guid.TryParse(dto.CommunityId, out var communityId))
                {
                    throw new NotImplementedException("社区信息不正确！");
                }
                var communitie = await db.Communities.Where(x => x.Id == communityId &&  x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (communitie == null)
                {
                    throw new NotImplementedException("社区信息不存在！");
                }

                if (!Guid.TryParse(dto.StreetOfficeId, out var streetOfficeId))
                {
                    throw new NotImplementedException("街道办信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == streetOfficeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("街道办信息不存在！");
                }

                var entity = db.Votes.Add(new Vote
                {
                    CommunityId = dto.CommunityId,
                    CommunityName = communitie.Name,
                    SmallDistrictId = dto.SmallDistrictId,
                    SmallDistrictName = smallDistrict.Name,
                    Deadline = dto.Deadline,
                    SmallDistrictArray = dto.SmallDistrictArray,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = streetOffice.Name,
                    Summary = dto.Summary,
                    Title = dto.Title,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VoteDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Vote>> GetAllAsync(VoteDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Vote> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Vote>> GetListAsync(VoteDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
