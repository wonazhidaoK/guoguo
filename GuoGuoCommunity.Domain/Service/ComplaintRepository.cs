using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class ComplaintRepository : IComplaintRepository
    {
        public async Task<Complaint> AddAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ComplaintTypeId, out var complaintTypeId))
                {
                    throw new NotImplementedException("投诉类型Id信息不正确！");
                }
                var complaintType = await db.ComplaintTypes.Where(x => x.Id == complaintTypeId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (complaintType == null)
                {
                    throw new NotImplementedException("投诉类型不存在！");
                }

                if (!Guid.TryParse(dto.OwnerCertificationId, out var ownerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == ownerCertificationId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主认证不存在！");
                }

                var entity = db.Complaints.Add(new Complaint
                {
                    CommunityId = ownerCertificationRecord.CommunityId,
                    CommunityName = ownerCertificationRecord.CommunityName,
                    SmallDistrictId = ownerCertificationRecord.SmallDistrictId,
                    SmallDistrictName = ownerCertificationRecord.SmallDistrictName,
                    StreetOfficeId = ownerCertificationRecord.StreetOfficeId,
                    StreetOfficeName = ownerCertificationRecord.StreetOfficeName,
                    ComplaintTypeId = complaintType.Id.ToString(),
                    ComplaintTypeName = complaintType.Name,
                    ProcessUpTime = DateTimeOffset.Now.AddDays(complaintType.ProcessingPeriod),
                    ExpiredTime = DateTimeOffset.Now.AddDays(complaintType.ComplaintPeriod),
                    DepartmentName = dto.DepartmentName,
                    DepartmentValue = dto.DepartmentValue,
                    Description = dto.Description,
                    OwnerCertificationId = dto.OwnerCertificationId,
                    StatusName = ComplaintStatus.NotAccepted.Name,
                    StatusValue = ComplaintStatus.NotAccepted.Value,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId,
                    OperationDepartmentName = dto.OperationDepartmentName,
                    OperationDepartmentValue = dto.OperationDepartmentValue
                });
                await db.SaveChangesAsync(token);
                return entity;
            }

            throw new NotImplementedException();
        }

        public async Task ClosedAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid guid))
                {
                    throw new NotImplementedException("投诉id信息不正确");
                }
                var entity = await db.Complaints.Where(x => x.IsDeleted == false && x.Id == guid && x.OwnerCertificationId == dto.OwnerCertificationId).FirstOrDefaultAsync(token);
                if (entity == null)
                {
                    throw new NotImplementedException("投诉信息不存在");
                }
                entity.LastOperationTime = dto.OperationTime;
                entity.LastOperationUserId = dto.OperationUserId;
                entity.ClosedTime = dto.OperationTime;
                entity.StatusValue = ComplaintStatus.Completed.Value;
                entity.StatusName = ComplaintStatus.Completed.Name;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task DeleteAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("投诉信息不正确！");
                }
                var complaint = await db.Complaints.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (complaint == null)
                {
                    throw new NotImplementedException("该投诉不存在！");
                }

                complaint.LastOperationTime = dto.OperationTime;
                complaint.LastOperationUserId = dto.OperationUserId;
                complaint.DeletedTime = dto.OperationTime;
                complaint.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<Complaint>> GetAllAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.Complaints.Where(x => x.IsDeleted == false && x.OwnerCertificationId == dto.OwnerCertificationId).ToListAsync(token);
            }
        }

        public async Task<List<Complaint>> GetAllForPropertyAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Complaints.Where(x => x.IsDeleted == false && x.DepartmentValue == Department.WuYe.Value && x.SmallDistrictId == dto.SmallDistrictId).ToListAsync(token);
                
                list = list.Where(x => x.CreateOperationTime >= dto.StartTime && x.CreateOperationTime <= dto.EndTime).ToList();
                if (!string.IsNullOrWhiteSpace(dto.StatusValue))
                {
                    list = list.Where(x => x.StatusValue == dto.StatusValue).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Description))
                {
                    list = list.Where(x => x.Description.Contains(dto.Description)).ToList();
                }
                return list;
            }
        }

        public async Task<List<Complaint>> GetAllForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.Complaints.Where(x => x.IsDeleted == false && x.DepartmentValue == Department.JieDaoBan.Value && x.StreetOfficeId == dto.StreetOfficeId).ToListAsync(token);
                //TODO 条件筛选 1.状态值 2时间段
                list = list.Where(x => x.CreateOperationTime >= dto.StartTime && x.CreateOperationTime <= dto.EndTime).ToList();
                if (!string.IsNullOrWhiteSpace(dto.StatusValue))
                {
                    list = list.Where(x => x.StatusValue == dto.StatusValue).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.Description))
                {
                    list = list.Where(x => x.Description.Contains(dto.Description)).ToList();
                }
                return list;
            }
        }

        public async Task<List<Complaint>> GetAllForVipOwnerAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.OwnerCertificationId, out var ownerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id不正确！");
                }
                var ownerCertificationRecord = await db.OwnerCertificationRecords.Where(x => x.Id == ownerCertificationId && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (ownerCertificationRecord == null)
                {
                    throw new NotImplementedException("业主认证信息不存在！");
                }
                return await db.Complaints.Where(x => x.IsDeleted == false && x.DepartmentValue == Department.YeZhuWeiYuanHui.Value && x.SmallDistrictId == ownerCertificationRecord.SmallDistrictId).ToListAsync(token);
            }
        }

        public async Task<Complaint> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Complaints.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该投诉Id信息不正确！");
            }
        }

        public Task<List<Complaint>> GetListAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task InvalidAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid guid))
                {
                    throw new NotImplementedException("投诉id信息不正确");
                }
                var entity = await db.Complaints.Where(x => x.IsDeleted == false && x.Id == guid ).FirstOrDefaultAsync(token);
                if (entity == null)
                {
                    throw new NotImplementedException("投诉信息不存在");
                }

                entity.LastOperationTime = dto.OperationTime;
                entity.LastOperationUserId = dto.OperationUserId;
                entity.IsInvalid = "true";
                entity.ClosedTime = dto.OperationTime;
                entity.StatusValue = ComplaintStatus.Completed.Value;
                entity.StatusName = ComplaintStatus.Completed.Name;
                await db.SaveChangesAsync(token);
            }
        }

        public Task UpdateAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateForAppealAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("投诉信息不正确！");
                }
                var complaint = await db.Complaints.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (complaint == null)
                {
                    throw new NotImplementedException("投诉信息不存在！");
                }

                complaint.DepartmentValue = Department.JieDaoBan.Value;
                complaint.DepartmentName = Department.JieDaoBan.Name;
                complaint.LastOperationTime = dto.OperationTime;
                complaint.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task UpdateForFinishedAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("投诉信息不正确！");
                }
                var complaint = await db.Complaints.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (complaint == null)
                {
                    throw new NotImplementedException("投诉信息不存在！");
                }

                complaint.StatusValue = ComplaintStatus.Finished.Value;
                complaint.StatusName = ComplaintStatus.Finished.Name;
                complaint.LastOperationTime = dto.OperationTime;
                complaint.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task UpdateForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("投诉信息不正确！");
                }
                var complaint = await db.Complaints.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (complaint == null)
                {
                    throw new NotImplementedException("投诉信息不存在！");
                }

                complaint.StatusValue = ComplaintStatus.Completed.Value;
                complaint.StatusName = ComplaintStatus.Completed.Name;
                complaint.LastOperationTime = dto.OperationTime;
                complaint.LastOperationUserId = dto.OperationUserId;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task ViewForPropertyAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid guid))
                {
                    throw new NotImplementedException("投诉id信息不正确");
                }
                var entity = await db.Complaints.Where(x => x.IsDeleted == false && x.Id == guid).FirstOrDefaultAsync(token);
                if (entity == null)
                {
                    throw new NotImplementedException("投诉信息不存在");
                }
                entity.LastOperationTime = dto.OperationTime;
                entity.LastOperationUserId = dto.OperationUserId;
                entity.StatusValue = ComplaintStatus.Processing.Value;
                entity.StatusName = ComplaintStatus.Processing.Name;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task ViewForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid guid))
                {
                    throw new NotImplementedException("投诉id信息不正确");
                }
                var entity = await db.Complaints.Where(x => x.IsDeleted == false && x.Id == guid).FirstOrDefaultAsync(token);
                if (entity == null)
                {
                    throw new NotImplementedException("投诉信息不存在");
                }
                entity.LastOperationTime = dto.OperationTime;
                entity.LastOperationUserId = dto.OperationUserId;
                entity.StatusValue = ComplaintStatus.Processing.Value;
                entity.StatusName = ComplaintStatus.Processing.Name;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task ViewForVipOwnerAsync(ComplaintDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out Guid guid))
                {
                    throw new NotImplementedException("投诉id信息不正确");
                }
                var entity = await db.Complaints.Where(x => x.IsDeleted == false && x.Id == guid).FirstOrDefaultAsync(token);
                if (entity == null)
                {
                    throw new NotImplementedException("投诉信息不存在");
                }
                entity.LastOperationTime = dto.OperationTime;
                entity.LastOperationUserId = dto.OperationUserId;
                entity.StatusValue = ComplaintStatus.Processing.Value;
                entity.StatusName = ComplaintStatus.Processing.Name;
                await db.SaveChangesAsync(token);
            }
        }
    }
}
