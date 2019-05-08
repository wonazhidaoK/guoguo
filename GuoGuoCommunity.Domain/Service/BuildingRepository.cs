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
    public class BuildingRepository : IBuildingRepository
    {
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
                   // SmallDistrictName = smallDistricts.Name,
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
                var list = await db.Buildings.Include(x=>x.SmallDistrict.Community.StreetOffice).Where(x => x.IsDeleted == false).ToListAsync(token);
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
                await OnUpdateAsync(db, building, token);
                await db.SaveChangesAsync(token);
            }
        }

        private async Task OnUpdateAsync(GuoGuoCommunityContext db, Building dto, CancellationToken token = default)
        {
            BuildingIncrementer incrementer = new BuildingIncrementer();
            //业主认证记录订阅
            OwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
            ownerCertificationRecordRepository.OnSubscribe(incrementer);
            //业户信息订阅
            IndustryRepository industryRepository = new IndustryRepository();
            industryRepository.OnSubscribe(incrementer);

            await incrementer.OnUpdate(db, dto, token);
        }

        private async Task<bool> OnDeleteAsync(GuoGuoCommunityContext db, BuildingDto dto, CancellationToken token = default)
        {
            //业户信息
            //if (await db.Industries.Where(x => x.BuildingId == dto.Id.ToString() && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            //{
            //    return true;
            //}

            //业主认证记录
            //if (await db.OwnerCertificationRecords.Where(x => x.BuildingId == dto.Id.ToString() && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            //{
            //    return true;
            //}
            return false;
        }

        public void OnSubscribe(SmallDistrictIncrementer incrementer)
        {
            incrementer.SmallDistrictEvent += SmallDistrictChanging;//在发布者私有委托里增加方法
        }

        public async void SmallDistrictChanging(GuoGuoCommunityContext dbs, SmallDistrict smallDistrict, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                //await db.Buildings.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new Building { SmallDistrictName = smallDistrict.Name });
            }
        }
    }
}
