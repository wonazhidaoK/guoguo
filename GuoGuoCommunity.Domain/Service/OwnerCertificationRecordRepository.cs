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
    public class OwnerCertificationRecordRepository : IOwnerCertificationRecordRepository
    {
        public async Task<OwnerCertificationRecord> AddAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                //用户信息
                if (!Guid.TryParse(dto.UserId, out var userId))
                {
                    throw new NotImplementedException("用户Id不正确！");
                }
                var user = await db.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("用户信息不存在！");
                }

                //街道办信息
                if (!Guid.TryParse(dto.StreetOfficeId, out var streetOfficeId))
                {
                    throw new NotImplementedException("街道办Id不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == streetOfficeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("街道办信息不存在！");
                }

                //社区信息
                if (!Guid.TryParse(dto.CommunityId, out var communityId))
                {
                    throw new NotImplementedException("社区Id不正确！");
                }
                var communitie = await db.Communities.Where(x => x.Id == communityId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (communitie == null)
                {
                    throw new NotImplementedException("社区信息不存在！");
                }

                //小区信息
                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区Id不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                //楼宇信息
                if (!Guid.TryParse(dto.BuildingId, out var buildingId))
                {
                    throw new NotImplementedException("楼宇Id不正确！");
                }
                var building = await db.Buildings.Where(x => x.Id == buildingId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (building == null)
                {
                    throw new NotImplementedException("楼宇信息不存在！");
                }

                //单元信息
                if (!Guid.TryParse(dto.BuildingUnitId, out var buildingUnitId))
                {
                    throw new NotImplementedException("楼宇单元Id不正确！");
                }
                var buildingUnit = await db.BuildingUnits.Where(x => x.Id == buildingUnitId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (buildingUnit == null)
                {
                    throw new NotImplementedException("楼宇单元信息不存在！");
                }

                //业户信息
                if (!Guid.TryParse(dto.IndustryId, out var industryId))
                {
                    throw new NotImplementedException("业户Id信息不正确！");
                }
                var industrie = await db.Industries.Where(x => x.Id == industryId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (industrie == null)
                {
                    throw new NotImplementedException("业户信息不存在！");
                }

                //&& x.IndustryId == dto.IndustryId && x.CertificationStatusValue != OwnerCertification.Failure.Value
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.UserId == dto.UserId  && x.IsDeleted == false && x.IsInvalid == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord != null)
                {
                    throw new NotImplementedException("该业主信息已存在！");
                }
                var entity = db.OwnerCertificationRecords.Add(new OwnerCertificationRecord
                {
                    CertificationResult = dto.CertificationResult,
                    UserId = dto.UserId,
                    IndustryId = dto.IndustryId,
                    SmallDistrictId = dto.SmallDistrictId,
                    StreetOfficeId = dto.StreetOfficeId,
                    BuildingUnitId = dto.BuildingUnitId,
                    BuildingId = dto.BuildingId,
                    CommunityId = dto.CommunityId,
                    BuildingName = building.Name,
                    BuildingUnitName = buildingUnit.UnitName,
                    CommunityName = communitie.Name,
                    IndustryName = industrie.Name,
                    SmallDistrictName = smallDistrict.Name,
                    StreetOfficeName = streetOffice.Name,
                    CertificationStatusName = OwnerCertification.Executing.Name,
                    CertificationStatusValue = OwnerCertification.Executing.Value,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("认证Id信息不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("该业主认证Id信息不存在！");
                }

                if (OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该业主信息下存在下级数据");
                }

                ownerCertificationRecord.LastOperationTime = dto.OperationTime;
                ownerCertificationRecord.LastOperationUserId = dto.OperationUserId;
                ownerCertificationRecord.DeletedTime = dto.OperationTime;
                ownerCertificationRecord.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<OwnerCertificationRecord>> GetAllAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerCertificationRecord> GetAsync(string id, CancellationToken token = default)
        {
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    if (Guid.TryParse(id, out var uid))
                    {
                        return await db.OwnerCertificationRecords.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                    }
                    throw new NotImplementedException("该认证Id信息不正确！");
                }
            }
            catch (Exception)
            {
                return new OwnerCertificationRecord();
            }
        }

        public async Task<List<OwnerCertificationRecord>> GetListAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.UserId == dto.UserId && x.IsInvalid == false).ToListAsync(token);
            }
        }

        public async Task<OwnerCertificationRecord> UpdateAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("认证信息Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("该认证信息不存在！");
                }
                ownerCertificationRecord.OwnerId = dto.OwnerId;
                ownerCertificationRecord.OwnerName = dto.OwnerName;
                ownerCertificationRecord.CertificationStatusName = dto.CertificationStatusName;
                ownerCertificationRecord.CertificationStatusValue = dto.CertificationStatusValue;
                ownerCertificationRecord.CertificationResult = dto.CertificationResult;
                ownerCertificationRecord.LastOperationTime = dto.OperationTime;
                ownerCertificationRecord.LastOperationUserId = dto.OperationUserId;
                ownerCertificationRecord.CertificationTime= dto.OperationTime;
                OnUpdate(db, dto, token);
                await db.SaveChangesAsync(token);
                return ownerCertificationRecord;
            }
        }
        
        public Task UpdateStatusAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OwnerCertificationRecord>> GetListForOwnerAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.OwnerId == dto.OwnerId).ToListAsync(token);
            }
        }

        public async Task UpdateInvalidAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("认证信息Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("该认证信息不存在！");
                }

                ownerCertificationRecord.IsInvalid = true;

                ownerCertificationRecord.LastOperationTime = dto.OperationTime;
                ownerCertificationRecord.LastOperationUserId = dto.OperationUserId;
                OnUpdate(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        #region 事件
        private void OnUpdate(GuoGuoCommunityContext db, OwnerCertificationRecordDto dto, CancellationToken token = default)
        {

        }

        private bool OnDelete(GuoGuoCommunityContext db, OwnerCertificationRecordDto dto, CancellationToken token = default)
        {

            return false;
        }

        public void OnSubscribe(StreetOfficeIncrementer incrementer)
        {
            incrementer.StreetOfficeEvent += StreetOfficeChanging;//在发布者私有委托里增加方法
        }

        public async void StreetOfficeChanging(GuoGuoCommunityContext dbs, StreetOffice streetOffice, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.OwnerCertificationRecords.Where(x => x.StreetOfficeId == streetOffice.Id.ToString()).UpdateAsync(x => new OwnerCertificationRecord { StreetOfficeName = streetOffice.Name });
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
                await db.OwnerCertificationRecords.Where(x => x.CommunityId == community.Id.ToString()).UpdateAsync(x => new OwnerCertificationRecord { CommunityName = community.Name });
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
                await db.OwnerCertificationRecords.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new OwnerCertificationRecord { SmallDistrictName = smallDistrict.Name });
            }
        }

        public void OnSubscribe(BuildingIncrementer incrementer)
        {
            incrementer.BuildingEvent += BuildingChanging;
        }

        public async void BuildingChanging(GuoGuoCommunityContext dbs, Building building, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.OwnerCertificationRecords.Where(x => x.BuildingId == building.Id.ToString()).UpdateAsync(x => new OwnerCertificationRecord { BuildingName = building.Name });
            }
        }

        public void OnSubscribe(BuildingUnitIncrementer incrementer)
        {
            incrementer.BuildingUnitEvent += BuildingUnitChanging;
        }

        public async void BuildingUnitChanging(GuoGuoCommunityContext dbs, BuildingUnit  buildingUnit, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.OwnerCertificationRecords.Where(x => x.BuildingUnitId == buildingUnit.Id.ToString()).UpdateAsync(x => new OwnerCertificationRecord {  BuildingUnitName = buildingUnit.UnitName });
            }
        }

        #endregion

        public async Task<List<OwnerCertificationRecord>> GetListForSmallDistrictIdAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId&&x.IsInvalid==false).ToListAsync(token);
            }
        }

        public async Task<List<OwnerCertificationRecord>> GetAllForSmallDistrictIdAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId&&x.UserId==dto.UserId).ToListAsync(token);
            }
        }

        public async Task<List<OwnerCertificationRecord>> GetListForIdArrayAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.OwnerCertificationRecords.Where(x => x.IsDeleted==false).ToListAsync(token);
                return (from x in list where ids.Contains(x.Id.ToString()) select x).ToList();
            }
        }
    }
}
