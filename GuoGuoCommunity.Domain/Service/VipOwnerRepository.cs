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
    public class VipOwnerRepository : IVipOwnerRepository
    {
        public async Task<VipOwner> AddAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            //TODO检查是否存在未竞选业委会信息
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.SmallDistrictId, out var smallDistrictId))
                {
                    throw new NotImplementedException("小区信息不正确！");
                }
                var smallDistrict = await db.SmallDistricts.Where(x => x.Id == smallDistrictId && x.Name == dto.SmallDistrictName && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (smallDistrict == null)
                {
                    throw new NotImplementedException("小区信息不存在！");
                }

                var vipOwners = await db.VipOwners.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId).FirstOrDefaultAsync(token);
                if (vipOwners != null)
                {
                    throw new NotImplementedException("该业委会已存在！");
                }
                var entity = db.VipOwners.Add(new VipOwner
                {
                    Name = dto.Name,
                    RemarkName = dto.RemarkName,
                    SmallDistrictId = dto.SmallDistrictId,
                    SmallDistrictName = dto.SmallDistrictName,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwners = await db.VipOwners.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (vipOwners == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }
                if (OnDeleteAsync(db, dto, token))
                {
                    throw new NotImplementedException("该业委会存在下级数据！");
                }
                vipOwners.LastOperationTime = dto.OperationTime;
                vipOwners.LastOperationUserId = dto.OperationUserId;
                vipOwners.DeletedTime = dto.OperationTime;
                vipOwners.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<VipOwner>> GetAllAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.VipOwners.Where(x => x.IsDeleted == false && x.IsValid == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    list = list.Where(x => x.SmallDistrictId == dto.SmallDistrictId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name) || x.RemarkName.Contains(dto.Name)).ToList();
                }
                //if (!string.IsNullOrWhiteSpace(dto.RemarkName))
                //{
                //    list = list.Where(x => x.RemarkName.Contains(dto.RemarkName)).ToList();
                //}
                return list;
            }
        }

        public async Task<VipOwner> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该业委会信息不正确！");
            }
        }

        public async Task<List<VipOwner>> GetIsValidAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (string.IsNullOrWhiteSpace(dto.SmallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息不正确！");
                }
                return await db.VipOwners.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);
            }
        }

        public async Task<List<VipOwner>> GetListAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwners.Where(x => x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId && x.IsValid == false && x.IsElection == false).ToListAsync(token);
            }
        }

        public async Task UpdateAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwner = await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwner == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }
                var entity = await db.VipOwners.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId && x.Id != uid).FirstOrDefaultAsync(token);
                if (entity != null)
                {
                    throw new NotImplementedException("该业委会已存在！");
                }
                vipOwner.RemarkName = dto.RemarkName;
                vipOwner.LastOperationTime = dto.OperationTime;
                vipOwner.LastOperationUserId = dto.OperationUserId;
                await OnUpdateAsync(db, vipOwner, token);
                await db.SaveChangesAsync(token);
            }
        }

        private async Task OnUpdateAsync(GuoGuoCommunityContext db, VipOwner dto, CancellationToken token = default)
        {
            VipOwnerIncrementer incrementer = new VipOwnerIncrementer();

            VipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository = new VipOwnerCertificationRecordRepository();
            vipOwnerCertificationRecordRepository.OnSubscribe(incrementer);

            await incrementer.OnUpdate(db, dto, token);
        }

        private bool OnDeleteAsync(GuoGuoCommunityContext db, VipOwnerDto dto, CancellationToken token = default)
        {

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
                await db.VipOwners.Where(x => x.SmallDistrictId == smallDistrict.Id.ToString()).UpdateAsync(x => new VipOwner { SmallDistrictName = smallDistrict.Name });
            }
        }

        public async Task<List<VipOwner>> GetListForStreetOfficeIdAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {

                var streetOfficeList = await db.SmallDistricts.Where(x => x.StreetOfficeId == dto.StreetOfficeId).Select(x => x.Id).ToListAsync(token);
                return await db.VipOwners.Where(x => streetOfficeList.Contains(x.Id)).ToListAsync(token);
            }
        }

        public async Task UpdateIsElectionAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwner = await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwner == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }
                var entity = await db.VipOwners.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.SmallDistrictId == dto.SmallDistrictId && x.Id != uid).FirstOrDefaultAsync(token);
                if (entity != null)
                {
                    throw new NotImplementedException("该业委会已存在！");
                }
                vipOwner.IsElection = true;
                vipOwner.LastOperationTime = dto.OperationTime;
                vipOwner.LastOperationUserId = dto.OperationUserId;
                await OnUpdateAsync(db, vipOwner, token);
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<VipOwner> GetForSmallDistrictIdAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwners.Where(x => x.SmallDistrictId == dto.SmallDistrictId && x.IsValid == true).FirstOrDefaultAsync(token);
            }
        }

        public async Task UpdateValidAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwner = await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwner == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }
               
                vipOwner.IsElection = true;
                vipOwner.LastOperationTime = dto.OperationTime;
                vipOwner.LastOperationUserId = dto.OperationUserId;
                await OnUpdateAsync(db, vipOwner, token);
                await db.SaveChangesAsync(token);
            }
        }

        public async Task UpdateInvalidAsync(VipOwnerDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("业委会Id信息不正确！");
                }
                var vipOwner = await db.VipOwners.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (vipOwner == null)
                {
                    throw new NotImplementedException("该业委会不存在！");
                }
                
                vipOwner.IsValid = false;
                vipOwner.LastOperationTime = dto.OperationTime;
                vipOwner.LastOperationUserId = dto.OperationUserId;
                await OnUpdateAsync(db, vipOwner, token);
                await db.SaveChangesAsync(token);
            }
        }
    }
}
