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
    public class StreetOfficeService : IStreetOfficeService
    {
        public async Task<StreetOffice> AddAsync(StreetOfficeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var streetOffice = await db.StreetOffices.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice != null)
                {
                    throw new NotImplementedException("该街道办已存在！");
                }
                var entity = db.StreetOffices.Add(new StreetOffice
                {
                    Name = dto.Name,
                    City = dto.City,
                    Region = dto.Region,
                    State = dto.State,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(StreetOfficeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("街道办信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("该街道办不存在！");
                }
                if (await OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该街道办存在下级社区数据！");
                }
                streetOffice.LastOperationTime = dto.OperationTime;
                streetOffice.LastOperationUserId = dto.OperationUserId;
                streetOffice.DeletedTime = dto.OperationTime;
                streetOffice.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<StreetOffice>> GetAllAsync(StreetOfficeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.StreetOffices.Where(x => x.IsDeleted == false).ToListAsync(token);
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
                    list = list.Where(x => x.Region == dto.Region).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                return list;
            }
        }

        public async Task<StreetOffice> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.StreetOffices.Where(x => x.Id == uid).FirstOrDefaultAsync(token);

                }
                throw new NotImplementedException("该街道办不存在！");
            }
        }

        public async Task<List<StreetOffice>> GetListAsync(StreetOfficeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.StreetOffices.Where(x => x.IsDeleted == false && x.Region == dto.Region&&x.State==dto.State&&x.City==dto.City).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(StreetOfficeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("街道办信息不正确！");
                }
                var streetOffice = await db.StreetOffices.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (streetOffice == null)
                {
                    throw new NotImplementedException("该街道办不存在！");
                }
                if (await db.StreetOffices.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该街道办名称已存在！");
                }
                streetOffice.Name = dto.Name;
                streetOffice.LastOperationTime = dto.OperationTime;
                streetOffice.LastOperationUserId = dto.OperationUserId;
                await OnUpdate(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        private async Task OnUpdate(GuoGuoCommunityContext db, StreetOfficeDto dto, CancellationToken token = default)
        {
            await db.Communities.Where(x => x.StreetOfficeId == dto.Id).UpdateAsync(x => new Community { StreetOfficeName = dto.Name });
            await db.SmallDistricts.Where(x => x.StreetOfficeId == dto.Id).UpdateAsync(x => new SmallDistrict { StreetOfficeName = dto.Name });
        }

        private async Task<bool> OnDelete(GuoGuoCommunityContext db, StreetOfficeDto dto, CancellationToken token = default)
        {
            if (await db.Communities.Where(x => x.StreetOfficeId == dto.Id.ToString() && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }
            return false;
        }
    }
}
