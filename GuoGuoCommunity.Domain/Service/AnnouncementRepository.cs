using EntityFramework.Extensions;
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

        public async Task<Announcement> AddVipOwnerAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.OwnerCertificationId, out var ownerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == ownerCertificationId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主认证信息不存在！");
                }

                var entity = db.Announcements.Add(new Announcement
                {
                    Content = dto.Content,
                    SmallDistrictArray = ownerCertificationRecord.SmallDistrictId.ToString(),
                    DepartmentName = dto.DepartmentName,
                    DepartmentValue = dto.DepartmentValue,
                    Summary = dto.Summary,
                    Title = dto.Title,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    CommunityId = ownerCertificationRecord.CommunityId,
                    CommunityName = ownerCertificationRecord.CommunityName,
                    SmallDistrictId = ownerCertificationRecord.SmallDistrictId,
                    SmallDistrictName = ownerCertificationRecord.SmallDistrictName,
                    StreetOfficeId = ownerCertificationRecord.StreetOfficeId,
                    StreetOfficeName = ownerCertificationRecord.StreetOfficeName,
                    OwnerCertificationId = dto.OwnerCertificationId
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
                if (!Guid.TryParse(dto.OwnerCertificationId, out var ownerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == ownerCertificationId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主认证信息不存在！");
                }
                var list = await db.Announcements.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                if (dto.DepartmentValue != Department.JieDaoBan.Value)
                {
                    list = list.Where(x => x.SmallDistrictArray == ownerCertificationRecord.SmallDistrictId).ToList();
                }
                else
                {
                    list = list.Where(x => x.SmallDistrictArray.Split(',').Contains(ownerCertificationRecord.SmallDistrictId)).ToList();
                }
                return list;
            }
        }

        public async Task<List<Announcement>> GetAllForVipOwnerAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.OwnerCertificationId, out var ownerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == ownerCertificationId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主认证信息不存在！");
                }
                var list = await db.Announcements.Where(x => x.IsDeleted == false && x.DepartmentValue == dto.DepartmentValue).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }

                list = list.Where(x => x.SmallDistrictArray == ownerCertificationRecord.SmallDistrictId).ToList();

                return list;
            }
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
                list = list.Where(x => x.CreateOperationTime >= dto.StartTime && x.CreateOperationTime <= dto.EndTime).ToList();

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
                list = list.Where(x => x.CreateOperationTime >= dto.StartTime && x.CreateOperationTime <= dto.EndTime).ToList();
                return list.Where(x => x.SmallDistrictId == dto.SmallDistrictId).ToList();
            }
        }

        public Task<Announcement> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AnnouncementDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        #region 事件

        public void OnSubscribe(StreetOfficeIncrementer incrementer)
        {
            incrementer.StreetOfficeEvent += StreetOfficeChanging;//在发布者私有委托里增加方法
        }

        public async void StreetOfficeChanging(GuoGuoCommunityContext dbs, StreetOffice streetOffice, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.Announcements.Where(x => x.StreetOfficeId == streetOffice.Id.ToString()).UpdateAsync(x => new Announcement { StreetOfficeName = streetOffice.Name });
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
                await db.Announcements.Where(x => x.CommunityId == community.Id.ToString()).UpdateAsync(x => new Announcement { CommunityName = community.Name });
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
                await db.Announcements.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new Announcement { SmallDistrictName = smallDistrict.Name });
            }
        }

        #endregion

    }
}
