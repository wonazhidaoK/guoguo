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
    public class ComplaintTypeService : IComplaintTypeService
    {
        public async Task<ComplaintType> AddAsync(ComplaintTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var complaintType = await db.ComplaintTypes.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (complaintType != null)
                {
                    throw new NotImplementedException("该投诉类型已存在！");
                }
                var entity = db.ComplaintTypes.Add(new ComplaintType
                {
                    Name = dto.Name,
                    Level = dto.Level,
                    ProcessingPeriod = "7",
                    ComplaintPeriod = "15",
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    Description = dto.Description,
                    InitiatingDepartmentValue = dto.InitiatingDepartmentValue,
                    InitiatingDepartmentName = dto.InitiatingDepartmentName,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(ComplaintTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("投诉类型信息不正确！");
                }
                var complaintType = await db.ComplaintTypes.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (complaintType == null)
                {
                    throw new NotImplementedException("该投诉类型不存在！");
                }

                complaintType.LastOperationTime = dto.OperationTime;
                complaintType.LastOperationUserId = dto.OperationUserId;
                complaintType.DeletedTime = dto.OperationTime;
                complaintType.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<ComplaintType>> GetAllAsync(ComplaintTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.ComplaintTypes.Where(x => x.IsDeleted == false).ToListAsync(token);
                if (!string.IsNullOrWhiteSpace(dto.Description))
                {
                    list = list.Where(x => x.Description.Contains(dto.Description)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.InitiatingDepartmentValue))
                {
                    list = list.Where(x => x.InitiatingDepartmentValue == dto.InitiatingDepartmentValue).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    list = list.Where(x => x.Name.Contains(dto.Name)).ToList();
                }
                return list;
            }
        }

        public async Task<ComplaintType> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var uid))
                {
                    return await db.ComplaintTypes.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该投诉类型不正确！");
            }
        }

        public async Task<List<ComplaintType>> GetListAsync(ComplaintTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (string.IsNullOrWhiteSpace(dto.InitiatingDepartmentValue))
                {
                    throw new NotImplementedException("发起部门值不正确！");
                }
                return await db.ComplaintTypes.Where(x => x.IsDeleted == false && x.InitiatingDepartmentValue == dto.InitiatingDepartmentValue).ToListAsync(token); ;
            }
        }

        public async Task UpdateAsync(ComplaintTypeDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("投诉类型信息不正确！");
                }
                var complaintType = await db.ComplaintTypes.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (complaintType == null)
                {
                    throw new NotImplementedException("该投诉类型信息不存在！");
                }
                if (await db.ComplaintTypes.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
                {
                    throw new NotImplementedException("该投诉类型信息已存在！");
                }
                complaintType.Name = dto.Name;
                complaintType.Level = dto.Level;
                complaintType.Description = dto.Description;
                //complaintTypes.ProcessingPeriod = dto.ProcessingPeriod;
                //complaintTypes.ComplaintPeriod = dto.ComplaintPeriod;
                complaintType.LastOperationTime = dto.OperationTime;
                complaintType.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }
    }
}
