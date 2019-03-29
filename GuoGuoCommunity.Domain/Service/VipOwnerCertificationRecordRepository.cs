﻿using EntityFramework.Extensions;
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
    public class VipOwnerCertificationRecordRepository : IVipOwnerCertificationRecordRepository
    {
        public async Task<VipOwnerCertificationRecord> AddAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.VipOwnerId, out var vipOwnerId))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwners = await db.VipOwners.Where(x => x.Id == vipOwnerId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwners == null)
                {
                    throw new NotImplementedException("业委会信息不存在！");
                }

                if (!Guid.TryParse(dto.VipOwnerStructureId, out var vipOwnerStructureId))
                {
                    throw new NotImplementedException("职能Id信息不正确！");
                }
                var vipOwnerStructure = await db.VipOwnerStructures.Where(x => x.Id == vipOwnerStructureId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwnerStructure == null)
                {
                    throw new NotImplementedException("职能信息不存在！");
                }

                if (!Guid.TryParse(dto.UserId, out var userId))
                {
                    throw new NotImplementedException("用户Id信息不正确！");
                }
                var user = await db.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (user == null)
                {
                    throw new NotImplementedException("用户信息不存在！");
                }

                var entity = db.VipOwnerCertificationRecords.Add(new VipOwnerCertificationRecord
                {
                    UserId = dto.UserId,
                    VipOwnerStructureId = dto.VipOwnerStructureId,
                    VipOwnerId = dto.VipOwnerId,
                    VipOwnerName = vipOwners.Name,
                    VipOwnerStructureName = vipOwnerStructure.Name,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationRecord>> GetAllAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerCertificationRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VipOwnerCertificationRecord>> GetListAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationRecords.Where(x => x.IsDeleted == false && x.UserId == dto.UserId).ToListAsync(token);
            }
        }

        public Task UpdateAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void OnSubscribe(VipOwnerIncrementer incrementer)
        {
            incrementer.VipOwnerEvent += VipOwnerChanging;//在发布者私有委托里增加方法
        }

        public async void VipOwnerChanging(GuoGuoCommunityContext dbs, VipOwner vipOwner, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.VipOwnerCertificationRecords.Where(x => x.VipOwnerId == vipOwner.Id.ToString()).UpdateAsync(x => new VipOwnerCertificationRecord { VipOwnerName = vipOwner.Name });
            }
        }

        public void OnSubscribe(VipOwnerStructureIncrementer incrementer)
        {
            incrementer.VipOwnerStructureEvent += VipOwnerStructureChanging;//在发布者私有委托里增加方法
        }

        public async void VipOwnerStructureChanging(GuoGuoCommunityContext dbs, VipOwnerStructure vipOwnerStructure, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.VipOwnerCertificationRecords.Where(x => x.VipOwnerStructureId == vipOwnerStructure.Id.ToString()).UpdateAsync(x => new VipOwnerCertificationRecord { VipOwnerStructureName = vipOwnerStructure.Name });
            }
        }
    }
}
