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
    public class VipOwnerStructureService : IVipOwnerStructureService
    {
        public async Task<VipOwnerStructure> AddAsync(VipOwnerStructureDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var community = await db.VipOwnerStructures.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (community != null)
                {
                    throw new NotImplementedException("该业委会职能已存在！");
                }
                var entity = db.VipOwnerStructures.Add(new VipOwnerStructure
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    IsReview = dto.IsReview.Value,
                    Weights = dto.Weights,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId

                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(VipOwnerStructureDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会职能信息不正确！");
                }
                var vipOwnerStructures = await db.VipOwnerStructures.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwnerStructures == null)
                {
                    throw new NotImplementedException("该业委会职能不存在！");
                }

                vipOwnerStructures.LastOperationTime = dto.OperationTime;
                vipOwnerStructures.LastOperationUserId = dto.OperationUserId;
                vipOwnerStructures.DeletedTime = dto.OperationTime;
                vipOwnerStructures.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<VipOwnerStructure>> GetAllAsync(VipOwnerStructureDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwnerStructures.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Description))
                {
                    list = list.Where(x => x.Description.Contains(dto.Description)).ToList();
                }
                if (dto.IsReview.HasValue)
                {
                    list = list.Where(x => x.IsReview == dto.IsReview).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Weights))
                {
                    list = list.Where(x => x.Weights == dto.Weights).ToList();
                }
                return list;
            }
        }

        public async Task<VipOwnerStructure> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var uid))
                {
                    return await db.VipOwnerStructures.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("业委会职能信息不正确！");
            }
        }

        public async Task<List<VipOwnerStructure>> GetListAsync(CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwnerStructures.Where(x => x.IsDeleted == false).ToListAsync(token);
                return list;
            }
        }

        public async Task UpdateAsync(VipOwnerStructureDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会职能信息不正确！");
                }
                var vipOwnerStructures = await db.VipOwnerStructures.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwnerStructures == null)
                {
                    throw new NotImplementedException("该业委会职能不存在！");
                }
                if (await db.VipOwnerStructures.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该业委会职能已存在！");
                }
                vipOwnerStructures.Name = dto.Name;
                vipOwnerStructures.Description = dto.Description;
                vipOwnerStructures.Weights = dto.Weights;
                vipOwnerStructures.IsReview = dto.IsReview.Value;
                vipOwnerStructures.LastOperationTime = dto.OperationTime;
                vipOwnerStructures.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }
    }
}
