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
    public class OwnerService : IOwnerService
    {
        public async Task<Owner> AddAsync(OwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.IndustryId, out var industryId))
                {
                    throw new NotImplementedException("业户Id信息不正确！");
                }
                var industrie = await db.Industries.Where(x => x.Id == industryId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (industrie == null)
                {
                    throw new NotImplementedException("业户信息不存在！");
                }

                var owner = await db.Owners.Where(x => x.IDCard == dto.IDCard && x.IsDeleted == false && x.IndustryId == dto.IndustryId).FirstOrDefaultAsync(token);
                if (owner != null)
                {
                    throw new NotImplementedException("该业主信息已存在！");
                }
                var entity = db.Owners.Add(new Owner
                {
                    Name = dto.Name,
                    Birthday = dto.Birthday,
                    Gender = dto.Gender,
                    IDCard = dto.IDCard,
                    PhoneNumber = dto.PhoneNumber,
                    IndustryId = dto.IndustryId,
                    IndustryName = industrie.Name,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(OwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业主信息不正确！");
                }
                var owner = await db.Owners.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (owner == null)
                {
                    throw new NotImplementedException("该业主信息不存在！");
                }

                if (OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该业主信息下存在下级数据");
                }

                owner.LastOperationTime = dto.OperationTime;
                owner.LastOperationUserId = dto.OperationUserId;
                owner.DeletedTime = dto.OperationTime;
                owner.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Owner>> GetAllAsync(OwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Owners.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.IDCard))
                {
                    list = list.Where(x => x.IDCard.Contains(dto.IDCard)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.IndustryId))
                {
                    list = list.Where(x => x.IndustryId == dto.IndustryId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                {
                    list = list.Where(x => x.PhoneNumber.Contains(dto.PhoneNumber)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Gender))
                {
                    list = list.Where(x => x.Gender == dto.Gender).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                return list;
            }
        }

        public async Task<Owner> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Owners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该业主信息不正确！");
            }
        }

        public async Task<List<Owner>> GetListAsync(OwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Owners.Where(x => x.IsDeleted == false && x.IndustryId == dto.IndustryId).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(OwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业主信息不正确！");
                }
                var owners = await db.Owners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (owners == null)
                {
                    throw new NotImplementedException("该社区不存在！");
                }

                if (await db.Owners.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.IndustryId == owners.IndustryId).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该社区名称已存在！");
                }
               
                owners.LastOperationTime = dto.OperationTime;
                owners.LastOperationUserId = dto.OperationUserId;
                OnUpdate(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        private void OnUpdate(GuoGuoCommunityContext db, OwnerDto dto, CancellationToken token = default)
        {

        }

        private bool OnDelete(GuoGuoCommunityContext db, OwnerDto dto, CancellationToken token = default)
        {

            return false;
        }
    }
}
