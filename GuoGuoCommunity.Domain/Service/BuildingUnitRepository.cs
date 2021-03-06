﻿using GuoGuoCommunity.Domain.Abstractions;
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
    public class BuildingUnitRepository : IBuildingUnitRepository
    {
        #region 事件

        private async Task<bool> OnDeleteAsync(GuoGuoCommunityContext db, BuildingUnitDto dto, CancellationToken token = default)
        {
            //业户信息
            if (await db.Industries.Where(x => x.BuildingUnitId.ToString() == dto.Id && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        public async Task<BuildingUnit> AddAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.BuildingId, out var buildingId))
                {
                    throw new NotImplementedException("楼宇Id信息不正确！");
                }
                var building = await db.Buildings.Where(x => x.Id == buildingId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (building == null)
                {
                    throw new NotImplementedException("楼宇信息不存在！");
                }

                var buildingUnits = await db.BuildingUnits.Where(x => x.UnitName == dto.UnitName && x.IsDeleted == false && x.BuildingId == buildingId).FirstOrDefaultAsync(token);
                if (buildingUnits != null)
                {
                    throw new NotImplementedException("该楼宇单元信息已存在！");
                }
                var entity = db.BuildingUnits.Add(new BuildingUnit
                {
                    UnitName = dto.UnitName,
                    NumberOfLayers = dto.NumberOfLayers,
                    BuildingId = buildingId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("楼宇单元Id信息不正确！");
                }
                var buildingUnit = await db.BuildingUnits.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (buildingUnit == null)
                {
                    throw new NotImplementedException("该楼宇单元信息不存在！");
                }

                if (await OnDeleteAsync(db, dto, token))
                {
                    throw new NotImplementedException("该楼宇单元信息存在下级数据！");
                }
                buildingUnit.LastOperationTime = dto.OperationTime;
                buildingUnit.LastOperationUserId = dto.OperationUserId;
                buildingUnit.DeletedTime = dto.OperationTime;
                buildingUnit.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<BuildingUnit>> GetAllAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.BuildingUnits.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.UnitName))
                {
                    list = list.Where(x => x.UnitName.Contains(dto.UnitName)).ToList();
                }
                if (dto.NumberOfLayers != 0)
                {
                    list = list.Where(x => x.NumberOfLayers == dto.NumberOfLayers).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.BuildingId))
                {
                    list = list.Where(x => x.BuildingId.ToString() == dto.BuildingId).ToList();
                }
                return list;
            }
        }

        public async Task<BuildingUnit> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.BuildingUnits.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该楼宇单元信息不正确！");
            }
        }

        public async Task<List<BuildingUnit>> GetListAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.BuildingUnits.Where(x => x.IsDeleted == false && x.BuildingId.ToString() == dto.BuildingId).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("楼宇单元Id信息不正确！");
                }
                var buildingUnit = await db.BuildingUnits.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (buildingUnit == null)
                {
                    throw new NotImplementedException("该楼宇单元信息不存在！");
                }

                if (await db.BuildingUnits.Where(x => x.UnitName == dto.UnitName && x.IsDeleted == false && x.BuildingId == buildingUnit.BuildingId && x.Id != uid).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该楼宇单元信息名称已存在！");
                }

                buildingUnit.UnitName = dto.UnitName;
                buildingUnit.NumberOfLayers = dto.NumberOfLayers;
                buildingUnit.LastOperationTime = dto.OperationTime;
                buildingUnit.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<BuildingUnit>> GetAllIncludeAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.BuildingUnits.Include(x => x.Building.SmallDistrict.Community.StreetOffice).Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.UnitName))
                {
                    list = list.Where(x => x.UnitName.Contains(dto.UnitName)).ToList();
                }
                if (dto.NumberOfLayers != 0)
                {
                    list = list.Where(x => x.NumberOfLayers == dto.NumberOfLayers).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.BuildingId))
                {
                    list = list.Where(x => x.BuildingId.ToString() == dto.BuildingId).ToList();
                }
                return list;
            }
        }

        public async Task<BuildingUnit> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.BuildingUnits.Include(x => x.Building.SmallDistrict.Community.StreetOffice).Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该楼宇单元信息不正确！");
            }
        }

        public async Task<List<BuildingUnit>> GetListIncludeAsync(BuildingUnitDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.BuildingUnits.Include(x => x.Building.SmallDistrict.Community.StreetOffice).Where(x => x.IsDeleted == false && x.BuildingId.ToString() == dto.BuildingId).ToListAsync(token);
            }
        }

      public async  Task<BuildingUnit> UpdateNumberOfLayersAsync(BuildingUnitDto dto, CancellationToken token)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("楼宇单元Id信息不正确！");
                }
                var buildingUnit = await db.BuildingUnits.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (buildingUnit == null)
                {
                    throw new NotImplementedException("该楼宇单元信息不存在！");
                }

                buildingUnit.NumberOfLayers = dto.NumberOfLayers;
                buildingUnit.LastOperationTime = dto.OperationTime;
                buildingUnit.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
                return buildingUnit;
            }
        }
    }
}
