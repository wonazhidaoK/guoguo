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

                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.UserId == dto.UserId && x.IndustryId == dto.IndustryId && x.CertificationStatusValue != OwnerCertification.Failure.Value && x.IsDeleted == false && x.IsInvalid == false).FirstOrDefaultAsync(token);
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
                    throw new NotImplementedException("该业主信息不存在！");
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

        public Task<OwnerCertificationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OwnerCertificationRecord>> GetListAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationRecords.Where(x => x.IsDeleted == false && x.UserId == dto.UserId & x.CertificationStatusValue == dto.CertificationStatusValue).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(OwnerCertificationRecordDto dto, CancellationToken token = default)
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

                ownerCertificationRecord.CertificationStatusName = dto.CertificationStatusName;
                ownerCertificationRecord.CertificationStatusValue = dto.CertificationStatusValue;
                ownerCertificationRecord.LastOperationTime = dto.OperationTime;
                ownerCertificationRecord.LastOperationUserId = dto.OperationUserId;
                OnUpdate(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        private void OnUpdate(GuoGuoCommunityContext db, OwnerCertificationRecordDto dto, CancellationToken token = default)
        {

        }

        private bool OnDelete(GuoGuoCommunityContext db, OwnerCertificationRecordDto dto, CancellationToken token = default)
        {

            return false;
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
    }
}
