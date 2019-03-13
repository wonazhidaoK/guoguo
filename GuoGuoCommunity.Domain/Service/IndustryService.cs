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
    public class IndustryService : IIndustryService
    {
        public async Task<Industry> AddAsync(IndustryDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.BuildingId, out var buildingId))
                {
                    throw new NotImplementedException("楼宇Id信息不正确！");
                }
                var buildings = await db.Buildings.Where(x => x.Id == buildingId && x.Name == dto.BuildingName && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (buildings == null)
                {
                    throw new NotImplementedException("楼宇信息不存在！");
                }
                if (!Guid.TryParse(dto.BuildingUnitId, out var buildingUnitId))
                {
                    throw new NotImplementedException("楼宇单元Id信息不正确！");
                }
                var buildingUnits = await db.BuildingUnits.Where(x => x.Id == buildingUnitId && x.UnitName == dto.BuildingUnitName && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (buildingUnits == null)
                {
                    throw new NotImplementedException("楼宇单元信息不存在！");
                }
                var industries = await db.Industries.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.BuildingId == dto.BuildingId).FirstOrDefaultAsync(token);
                if (industries != null)
                {
                    throw new NotImplementedException("该业户信息已存在！");
                }
                var entity = db.Industries.Add(new  Industry
                {
                    Name = dto.Name,
                    BuildingId=dto.BuildingId,
                    BuildingUnitName=dto.BuildingUnitName,
                    BuildingUnitId=dto.BuildingUnitId,
                    BuildingName=dto.BuildingName,
                    Acreage=dto.Acreage, 
                    NumberOfLayers=dto.NumberOfLayers,
                    Oriented=dto.Oriented,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(IndustryDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Industry>> GetAllAsync(IndustryDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Industry> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Industry>> GetListAsync(IndustryDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IndustryDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
