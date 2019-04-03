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
    public class ComplaintFollowUpRepository : IComplaintFollowUpRepository
    {
        public async Task<ComplaintFollowUp> AddAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ComplaintId, out var complaintId))
                {
                    throw new NotImplementedException("投诉Id信息不正确！");
                }

                var complaintFollowUp = await db.ComplaintFollowUps.Where(x => x.Id == complaintId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (complaintFollowUp == null)
                {
                    throw new NotImplementedException("投诉不存在！");
                }

                var entity = db.ComplaintFollowUps.Add(new ComplaintFollowUp
                {
                    ComplaintId = dto.ComplaintId,
                    Description = dto.Description,
                    OperationDepartmentName = dto.OperationDepartmentName,
                    OperationDepartmentValue = dto.OperationDepartmentValue,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    OwnerCertificationId = dto.OwnerCertificationId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
            throw new NotImplementedException();
        }

        public Task<ComplaintFollowUp> ClosedAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintFollowUp>> GetAllAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ComplaintFollowUp> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.ComplaintFollowUps.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该投诉Id信息不正确！");
            }
        }

        public async Task<List<ComplaintFollowUp>> GetListAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.ComplaintFollowUps.Where(x => x.IsDeleted == false && x.ComplaintId == dto.ComplaintId&&x.OwnerCertificationId==dto.OwnerCertificationId).ToListAsync(token);
            }
        }

        public Task UpdateAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
