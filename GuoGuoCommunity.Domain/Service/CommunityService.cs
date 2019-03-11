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
    public class CommunityService : ICommunityService
    {
        public async Task<Community> AddAsync(CommunityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.StreetOfficeId, out var streetOfficeId))
                {
                    throw new NotImplementedException("街道办信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == streetOfficeId && x.Name == dto.StreetOfficeName && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("街道办信息不存在！");
                }
                var community = await db.Communities.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.StreetOfficeId == dto.StreetOfficeId).FirstOrDefaultAsync(token);
                if (community != null)
                {
                    throw new NotImplementedException("该社区已存在！");
                }
                var entity = db.Communities.Add(new Community
                {
                    Name = dto.Name,
                    City = dto.City,
                    Region = dto.Region,
                    State = dto.State,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = dto.StreetOfficeName
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(CommunityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("社区信息不正确！");
                }
                var community = await db.Communities.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (community == null)
                {
                    throw new NotImplementedException("该社区不存在！");
                }

                community.LastOperationTime = dto.OperationTime;
                community.LastOperationUserId = dto.OperationUserId;
                community.DeletedTime = dto.OperationTime;
                community.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Community>> GetAllAsync(CommunityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Communities.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.State))
                {
                    list = list.Where(x => x.State==dto.State).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.City))
                {
                    list = list.Where(x => x.City==dto.City).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Region))
                {
                    list = list.Where(x => x.Region==dto.Region).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.StreetOfficeId))
                {
                    list = list.Where(x => x.StreetOfficeId == dto.StreetOfficeId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }

                return list;
            }
        }

        public async Task<Community> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var uid))
                {
                    return await db.Communities.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该社区信息不正确！");
            }
        }

        public async Task<List<Community>> GetListAsync(CommunityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Communities.Where(x => x.IsDeleted == false).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.StreetOfficeId))
                {
                    list = list.Where(x => x.StreetOfficeId == dto.StreetOfficeId).ToList();
                }
                return list;
            }
        }

        public async Task UpdateAsync(CommunityDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("社区信息不正确！");
                }
                var community = await db.Communities.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (community == null)
                {
                    throw new NotImplementedException("该社区不存在！");
                }
                if (await db.Communities.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.StreetOfficeId == community.StreetOfficeId).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该社区名称已存在！");
                }
                community.Name = dto.Name;
                community.LastOperationTime = dto.OperationTime;
                community.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }
    }
}
