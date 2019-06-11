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
    public class SmallDistrictRepository : ISmallDistrictRepository
    {
        #region 事件
        private async Task OnUpdate(GuoGuoCommunityContext db, SmallDistrict dto, CancellationToken token = default)
        {
            SmallDistrictIncrementer incrementer = new SmallDistrictIncrementer();

            //公告订阅
            AnnouncementRepository announcementRepository = new AnnouncementRepository();
            announcementRepository.OnSubscribe(incrementer);

            //投票订阅
            VoteRepository voteRepository = new VoteRepository();
            voteRepository.OnSubscribe(incrementer);

            //业委会成员申请表
            VipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository = new VipOwnerApplicationRecordRepository();
            vipOwnerApplicationRecordRepository.OnSubscribe(incrementer);

            //用户
            UserRepository userRepository = new UserRepository();
            userRepository.OnSubscribe(incrementer);

            await incrementer.OnUpdate(db, dto, token);
        }

        private async Task<bool> OnDelete(GuoGuoCommunityContext db, SmallDistrictDto dto, CancellationToken token = default)
        {
            //楼宇
            if (await db.Buildings.Where(x => x.SmallDistrictId.ToString() == dto.Id && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            //业委会
            if (await db.VipOwners.Where(x => x.IsDeleted == false && x.SmallDistrictId.ToString() == dto.Id).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            //公告
            if (await db.Announcements.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.Id).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            //投票
            if (await db.Votes.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.Id).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            //业委会成员申请
            if (await db.VipOwnerApplicationRecords.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.Id).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            //用户
            if (await db.Users.Where(x => x.SmallDistrictId == dto.Id.ToString() && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            //小区物业商户
            if (await db.SmallDistrictShops.Where(x => x.SmallDistrictId.ToString() == dto.Id.ToString() && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        public async Task<SmallDistrict> AddAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.StreetOfficeId, out var streetOfficeId))
                {
                    throw new NotImplementedException("街道办信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == streetOfficeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("街道办信息不存在！");
                }

                if (!Guid.TryParse(dto.CommunityId, out var communityId))
                {
                    throw new NotImplementedException("社区信息不正确！");
                }
                var communities = await db.Communities.Where(x => x.Id == communityId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (communities == null)
                {
                    throw new NotImplementedException("社区办信息不存在！");
                }

                var smallDistricts = await db.SmallDistricts.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.CommunityId == communityId).FirstOrDefaultAsync(token);
                if (smallDistricts != null)
                {
                    throw new NotImplementedException("该小区已存在！");
                }
                var smallDistrict = new SmallDistrict
                {
                    Name = dto.Name,
                    City = dto.City,
                    Region = dto.Region,
                    State = dto.State,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    CommunityId = communityId,
                    PhoneNumber = dto.PhoneNumber
                };
                if (!string.IsNullOrWhiteSpace(dto.PropertyCompanyId))
                {
                    if (!Guid.TryParse(dto.PropertyCompanyId, out var propertyCompanyId))
                    {
                        throw new NotImplementedException("物业公司Id信息不正确！");
                    }
                    var propertyCompany = await db.PropertyCompanies.Where(x => x.Id == propertyCompanyId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                    if (propertyCompany == null)
                    {
                        throw new NotImplementedException("物业公司不存在！");
                    }
                    smallDistrict.PropertyCompanyId = propertyCompanyId;
                }
                var entity = db.SmallDistricts.Add(smallDistrict);
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("小区信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("该小区不存在！");
                }
                if (await OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该小区下存在下级业务数据");
                }
                smallDistrict.LastOperationTime = dto.OperationTime;
                smallDistrict.LastOperationUserId = dto.OperationUserId;
                smallDistrict.DeletedTime = dto.OperationTime;
                smallDistrict.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<SmallDistrict>> GetAllAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.SmallDistricts.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.State))
                {
                    list = list.Where(x => x.State == dto.State).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.City))
                {
                    list = list.Where(x => x.City == dto.City).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Region))
                {
                    list = list.Where(x => x.Region.Contains(dto.Region)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.StreetOfficeId))
                {
                    list = list.Where(x => x.Community.StreetOfficeId.ToString() == dto.StreetOfficeId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.CommunityId))
                {
                    list = list.Where(x => x.CommunityId.ToString() == dto.CommunityId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }

                return list;
            }
        }

        public async Task<SmallDistrict> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.SmallDistricts.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该小区信息不正确！");
            }
        }

        public async Task<List<SmallDistrict>> GetListAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.SmallDistricts.Where(x => x.IsDeleted == false && x.CommunityId.ToString() == dto.CommunityId).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("小区信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Include(x => x.Community).Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("该小区不存在！");
                }
                if (await db.SmallDistricts.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.Community.StreetOfficeId == smallDistrict.Community.StreetOfficeId && x.Id != uid).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该小区名称已存在！");
                }
                if (!string.IsNullOrWhiteSpace(dto.PropertyCompanyId))
                {
                    if (!Guid.TryParse(dto.PropertyCompanyId, out var propertyCompanyId))
                    {
                        throw new NotImplementedException("物业公司Id信息不正确！");
                    }
                    var propertyCompany = await db.PropertyCompanies.Where(x => x.Id == propertyCompanyId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                    if (propertyCompany == null)
                    {
                        throw new NotImplementedException("物业公司不存在！");
                    }
                    smallDistrict.PropertyCompanyId = propertyCompanyId;
                }
                smallDistrict.Name = dto.Name;
                smallDistrict.LastOperationTime = dto.OperationTime;
                smallDistrict.LastOperationUserId = dto.OperationUserId;
                smallDistrict.PhoneNumber = dto.PhoneNumber;
                await OnUpdate(db, smallDistrict, token);
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<SmallDistrict>> GetForIdsIncludeAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await (from x in db.SmallDistricts.Include(x => x.Community.StreetOffice) where ids.Contains(x.Id.ToString()) select x).ToListAsync(token);
            }
        }

        public async Task<List<SmallDistrict>> GetAllIncludeAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.SmallDistricts.Include(x => x.Community.StreetOffice).Include(x => x.PropertyCompany).Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.State))
                {
                    list = list.Where(x => x.State == dto.State).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.City))
                {
                    list = list.Where(x => x.City == dto.City).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Region))
                {
                    list = list.Where(x => x.Region.Contains(dto.Region)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.StreetOfficeId))
                {
                    list = list.Where(x => x.Community.StreetOfficeId.ToString() == dto.StreetOfficeId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.CommunityId))
                {
                    list = list.Where(x => x.CommunityId.ToString() == dto.CommunityId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.PropertyCompanyId))
                {
                    list = list.Where(x => x.PropertyCompanyId.ToString() == dto.PropertyCompanyId).ToList();
                }
                return list;
            }
        }

        public async Task<SmallDistrict> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.SmallDistricts.Include(x => x.Community.StreetOffice).Include(x => x.PropertyCompany).Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该小区信息不正确！");
            }
        }

        public async Task<List<SmallDistrict>> GetListIncludeAsync(SmallDistrictDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.SmallDistricts.Include(x => x.Community.StreetOffice).Where(x => x.IsDeleted == false && x.CommunityId.ToString() == dto.CommunityId).ToListAsync(token);
            }
        }
    }
}
