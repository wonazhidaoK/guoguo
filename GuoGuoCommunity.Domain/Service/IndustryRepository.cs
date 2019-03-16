using EntityFramework.Extensions;
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
    public class IndustryRepository : IIndustryRepository
    {
        public async Task<Industry> AddAsync(IndustryDto dto, CancellationToken token = default)
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
                if (!Guid.TryParse(dto.BuildingUnitId, out var buildingUnitId))
                {
                    throw new NotImplementedException("楼宇单元Id信息不正确！");
                }
                var buildingUnit = await db.BuildingUnits.Where(x => x.Id == buildingUnitId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (buildingUnit == null)
                {
                    throw new NotImplementedException("楼宇单元信息不存在！");
                }
                var industries = await db.Industries.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.BuildingId == dto.BuildingId).FirstOrDefaultAsync(token);
                if (industries != null)
                {
                    throw new NotImplementedException("该业户信息已存在！");
                }
                var entity = db.Industries.Add(new Industry
                {
                    Name = dto.Name,
                    BuildingId = dto.BuildingId,
                    BuildingUnitName = buildingUnit.UnitName,
                    BuildingUnitId = dto.BuildingUnitId,
                    BuildingName = building.Name,
                    Acreage = dto.Acreage,
                    NumberOfLayers = dto.NumberOfLayers,
                    Oriented = dto.Oriented,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(IndustryDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业户Id信息不正确！");
                }
                var industrie = await db.Industries.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (industrie == null)
                {
                    throw new NotImplementedException("该业户不存在！");
                }

                if (await OnDeleteAsync(db, dto, token))
                {
                    throw new NotImplementedException("该业户下存在下级数据");
                }

                industrie.LastOperationTime = dto.OperationTime;
                industrie.LastOperationUserId = dto.OperationUserId;
                industrie.DeletedTime = dto.OperationTime;
                industrie.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Industry>> GetAllAsync(IndustryDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Industries.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.BuildingId))
                {
                    list = list.Where(x => x.BuildingId == dto.BuildingId).ToList();
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }

                return list;
            }
        }

        public async Task<Industry> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Industries.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该业户信息不正确！");
            }
        }

        public async Task<List<Industry>> GetListAsync(IndustryDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Industries.Where(x => x.IsDeleted == false && x.BuildingId == dto.BuildingId).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(IndustryDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业户信息不正确！");
                }
                var industrie = await db.Industries.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (industrie == null)
                {
                    throw new NotImplementedException("该业户不存在！");
                }

                if (await db.Industries.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.BuildingId == industrie.BuildingId).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该业户名称已存在！");
                }
                industrie.Name = dto.Name;
                industrie.LastOperationTime = dto.OperationTime;
                industrie.LastOperationUserId = dto.OperationUserId;
                await OnUpdateAsync(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        private async Task OnUpdateAsync(GuoGuoCommunityContext db, IndustryDto dto, CancellationToken token = default)
        {
            await db.Owners.Where(x => x.IndustryId == dto.Id).UpdateAsync(x => new Owner { IndustryName = dto.Name });
        }

        private async Task<bool> OnDeleteAsync(GuoGuoCommunityContext db, IndustryDto dto, CancellationToken token = default)
        {
            if (await db.Owners.Where(x => x.IndustryId == dto.Id.ToString() && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }
            return false;
        }
    }
}
