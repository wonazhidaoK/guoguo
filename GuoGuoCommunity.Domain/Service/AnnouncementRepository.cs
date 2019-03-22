﻿using GuoGuoCommunity.Domain.Abstractions;
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
    public class AnnouncementRepository : IAnnouncementRepository
    {
        public async Task<Announcement> AddAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.Announcements.Add(new Announcement
                {
                    Content = dto.Content,
                    SmallDistrictArray = dto.SmallDistrictArray,
                    DepartmentName = dto.DepartmentName,
                    DepartmentValue = dto.DepartmentValue,
                    Summary = dto.Summary,
                    Title = dto.Title,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    CommunityId = dto.CommunityId,
                    CommunityName = dto.CommunityName,
                    SmallDistrictId = dto.SmallDistrictId,
                    SmallDistrictName = dto.SmallDistrictName,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = dto.StreetOfficeName
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("公告信息不正确！");
                }
                var announcement = await db.Announcements.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (announcement == null)
                {
                    throw new NotImplementedException("该公告不存在！");
                }

                announcement.LastOperationTime = dto.OperationTime;
                announcement.LastOperationUserId = dto.OperationUserId;
                announcement.DeletedTime = dto.OperationTime;
                announcement.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Announcement>> GetAllAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Announcements.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                if (dto.DepartmentValue != Department.JieDaoBan.Value)
                {
                    list = list.Where(x => x.SmallDistrictArray == dto.SmallDistrictArray).ToList();
                }
                else
                {
                    list = list.Where(x => x.SmallDistrictArray.Split(',').Contains(dto.SmallDistrictArray)).ToList();
                }
                return list;
            }
        }

        public Task<Announcement> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }



        public async Task<List<Announcement>> GetListForStreetOfficeAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Announcements.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }

                return list.Where(x => x.StreetOfficeId == dto.StreetOfficeId).ToList();
            }
        }

        public async Task<List<Announcement>> GetListPropertyAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Announcements.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                if (dto.DepartmentValue != Department.JieDaoBan.Value)
                {
                    list = list.Where(x => x.StreetOfficeId == dto.SmallDistrictArray).ToList();
                }
                return list.Where(x => x.SmallDistrictId == dto.SmallDistrictId).ToList();
            }
        }

        public Task UpdateAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
