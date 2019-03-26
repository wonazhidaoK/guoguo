﻿using EntityFramework.Extensions;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
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
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                if (!Guid.TryParse(dto.CommunityId, out var communityId))
                {
                    throw new NotImplementedException("社区信息不正确！");
                }
                var communitie = await db.Communities.Where(x => x.Id == communityId && x.IsDeleted == false).FirstOrDefaultAsync(token);
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

        public async Task<List<Vote>> GetAllForStreetOfficeAsync(VoteDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Votes.Where(x => x.IsDeleted == false && x.StreetOfficeId == dto.StreetOfficeId).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictArray))
                {
                    list = list.Where(x => x.SmallDistrictArray.Split(',').Contains(dto.SmallDistrictArray)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                return list;
            }
        }

        public async Task<List<Vote>> GetAllForPropertyAsync(VoteDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Votes.Where(x => x.IsDeleted == false&&x.DepartmentValue== Department.WuYe.Value && x.StreetOfficeId == dto.StreetOfficeId && x.CommunityId == dto.CommunityId && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                return list;
            }
        }

        public async Task<List<Vote>> GetAllForVipOwnerAsync(VoteDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Votes.Where(x => x.IsDeleted == false && x.DepartmentValue == Department.YeZhuWeiYuanHui.Value && x.StreetOfficeId == dto.StreetOfficeId && x.CommunityId == dto.CommunityId && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                return list;
            }
        }

        public async Task<List<Vote>> GetAllForOwnerAsync(VoteDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Votes.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue && x.StreetOfficeId == dto.StreetOfficeId && x.CommunityId == dto.CommunityId && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                return list;
            }
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

        public void OnSubscribe(StreetOfficeIncrementer incrementer)
        {
            incrementer.StreetOfficeEvent += StreetOfficeChanging;//在发布者私有委托里增加方法
        }

        public async void StreetOfficeChanging(GuoGuoCommunityContext dbs, StreetOffice streetOffice, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Votes.Where(x => x.StreetOfficeId == streetOffice.Id.ToString()).UpdateAsync(x => new Vote { StreetOfficeName = streetOffice.Name });
            }
        }

        public void OnSubscribe(CommunityIncrementer incrementer)
        {
            incrementer.CommunityEvent += CommunityChanging;//在发布者私有委托里增加方法
        }

        public async void CommunityChanging(GuoGuoCommunityContext dbs, Community community, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Votes.Where(x => x.CommunityId == community.Id.ToString()).UpdateAsync(x => new Vote { CommunityName = community.Name });
            }
        }

        public void OnSubscribe(SmallDistrictIncrementer incrementer)
        {
            incrementer.SmallDistrictEvent += SmallDistrictChanging;//在发布者私有委托里增加方法
        }

        public async void SmallDistrictChanging(GuoGuoCommunityContext dbs, SmallDistrict smallDistrict, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Votes.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new Vote { SmallDistrictName = smallDistrict.Name });
            }
        }

       
    }
}