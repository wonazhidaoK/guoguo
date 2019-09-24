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
    class VipOwnerCertificationConditionRepository : IVipOwnerCertificationConditionRepository
    {
        public async Task<VipOwnerCertificationCondition> AddAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var vipOwnerCertificationCondition = await db.VipOwnerCertificationConditions.Where(x => x.Title == dto.Title && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwnerCertificationCondition != null)
                {
                    throw new NotImplementedException("高级认证申请条件已存在！");
                }
                var entity = db.VipOwnerCertificationConditions.Add(new VipOwnerCertificationCondition
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    TypeName = dto.TypeName,
                    TypeValue = dto.TypeValue,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("高级认证申请条件Id信息不正确！");
                }
                var vipOwnerCertificationCondition = await db.VipOwnerCertificationConditions.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwnerCertificationCondition == null)
                {
                    throw new NotImplementedException("高级认证申请条件不存在！");
                }

                vipOwnerCertificationCondition.LastOperationTime = dto.OperationTime;
                vipOwnerCertificationCondition.LastOperationUserId = dto.OperationUserId;
                vipOwnerCertificationCondition.DeletedTime = dto.OperationTime;
                vipOwnerCertificationCondition.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<VipOwnerCertificationCondition>> GetAllAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwnerCertificationConditions.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.TypeValue))
                {
                    list = list.Where(x => x.TypeValue == dto.TypeValue).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    list = list.Where(x => x.Title.Contains(dto.Title)).ToList();
                }
                return list;
            }
        }

        public async Task<VipOwnerCertificationCondition> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.VipOwnerCertificationConditions.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该高级认证申请条件Id信息不正确！");
            }
        }

        public async Task<List<VipOwnerCertificationCondition>> GetListAsync(CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationConditions.Where(x => x.IsDeleted == false).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("高级认证申请条件Id信息不正确！");
                }
                var vipOwnerCertificationCondition = await db.VipOwnerCertificationConditions.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwnerCertificationCondition == null)
                {
                    throw new NotImplementedException("该高级认证申请条件不存在！");
                }
                if (await db.VipOwnerCertificationConditions.Where(x => x.Title == dto.Title && x.IsDeleted == false && x.Id != vipOwnerCertificationCondition.Id).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该高级认证申请条件名称已存在！");
                }

                vipOwnerCertificationCondition.Title = dto.Title;
                vipOwnerCertificationCondition.Description = dto.Description;
                vipOwnerCertificationCondition.LastOperationTime = dto.OperationTime;
                vipOwnerCertificationCondition.LastOperationUserId = dto.OperationUserId;

                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<VipOwnerCertificationCondition>> GetAllIncludeAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerCertificationCondition> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationCondition>> GetListIncludeAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationCondition>> GetListAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
