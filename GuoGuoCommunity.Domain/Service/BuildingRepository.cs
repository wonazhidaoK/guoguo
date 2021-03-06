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
    public class BuildingRepository : IBuildingRepository
    {

        #region 事件

        private async Task<bool> OnDeleteAsync(GuoGuoCommunityContext db, BuildingDto dto, CancellationToken token = default)
        {
            //楼宇单元信息
            if (await db.BuildingUnits.Where(x => x.BuildingId.ToString() == dto.Id && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        public async Task<Building> AddAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区信息不正确！");
                }
                var smallDistricts = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistricts == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                var duilding = await db.Buildings.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.SmallDistrictId == smallDistrictId).FirstOrDefaultAsync(token);
                if (duilding != null)
                {
                    throw new NotImplementedException("该楼宇信息已存在！");
                }
                var entity = db.Buildings.Add(new Building
                {
                    Name = dto.Name,
                    SmallDistrictId = smallDistrictId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("楼宇信息不正确！");
                }
                var building = await db.Buildings.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (building == null)
                {
                    throw new NotImplementedException("该楼宇不存在！");
                }

                if (await OnDeleteAsync(db, dto, token))
                {
                    throw new NotImplementedException("该楼宇信息存在下级数据！");
                }

                building.LastOperationTime = dto.OperationTime;
                building.LastOperationUserId = dto.OperationUserId;
                building.DeletedTime = dto.OperationTime;
                building.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Building>> GetAllAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Buildings.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    list = list.Where(x => x.SmallDistrictId.ToString() == dto.SmallDistrictId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                return list;
            }
        }

        public async Task<Building> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Buildings.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该楼宇Id信息不正确！");
            }
        }

        public async Task<List<Building>> GetListAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Buildings.Where(x => x.IsDeleted == false && x.SmallDistrictId.ToString() == dto.SmallDistrictId).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("楼宇信息不正确！");
                }
                var building = await db.Buildings.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (building == null)
                {
                    throw new NotImplementedException("该楼宇不存在！");
                }
                if (await db.Buildings.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.SmallDistrictId == building.SmallDistrictId && x.Id != uid).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该楼宇名称已存在！");
                }
                building.Name = dto.Name;
                building.LastOperationTime = dto.OperationTime;
                building.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Building>> GetAllIncludeAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Buildings.Include(x => x.SmallDistrict.Community.StreetOffice).Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    list = list.Where(x => x.SmallDistrictId.ToString() == dto.SmallDistrictId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                return list;
            }
        }

        public async Task<Building> GetIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Buildings.Include(x => x.SmallDistrict.Community.StreetOffice).Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该楼宇Id信息不正确！");
            }
        }

        public async Task<List<Building>> GetListIncludeAsync(BuildingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Buildings.Include(x => x.SmallDistrict.Community.StreetOffice).Where(x => x.IsDeleted == false && x.SmallDistrictId.ToString() == dto.SmallDistrictId).ToListAsync(token);
            }
        }
    }
}
