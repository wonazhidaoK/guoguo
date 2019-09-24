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
    public class ComplaintAnnexRepository : IComplaintAnnexRepository
    {
        public async Task<ComplaintAnnex> AddAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ComplaintId, out var complaintId))
                {
                    throw new NotImplementedException("投诉id信息不正确！");
                }
                var entity = db.ComplaintAnnices.Add(new ComplaintAnnex
                {
                    // ComplaintId = complaintId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                if (!string.IsNullOrWhiteSpace(dto.ComplaintFollowUpId))
                {
                    if (!Guid.TryParse(dto.ComplaintFollowUpId, out var complaintFollowUpId))
                    {
                        throw new NotImplementedException("投诉跟进id信息不正确！");
                    }
                    entity.ComplaintFollowUpId = complaintFollowUpId;
                }
                if (!Guid.TryParse(dto.AnnexContent, out var annexContent))
                {
                    throw new NotImplementedException("投诉附件id信息不正确！");
                }
                var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                entity.AnnexId = annexContent;
                entity.AnnexContent = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;

                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task<ComplaintAnnex> AddForFollowUpIdAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.ComplaintId, out var complaintId))
                {
                    throw new NotImplementedException("投诉id信息不正确！");
                }

                var entity = db.ComplaintAnnices.Add(new ComplaintAnnex
                {
                    // ComplaintId = complaintId,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    // ComplaintFollowUpId = dto.ComplaintFollowUpId
                });
                if (!string.IsNullOrWhiteSpace(dto.ComplaintFollowUpId))
                {
                    if (!Guid.TryParse(dto.ComplaintFollowUpId, out var complaintFollowUpId))
                    {
                        throw new NotImplementedException("投诉跟进id信息不正确！");
                    }
                    entity.ComplaintFollowUpId = complaintFollowUpId;
                }
                if (!Guid.TryParse(dto.AnnexContent, out var annexContent))
                {
                    throw new NotImplementedException("投诉附件id信息不正确！");
                }
                var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                entity.AnnexId = annexContent;
                entity.AnnexContent = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;

                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintAnnex>> GetAllAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintAnnex>> GetAllIncludeAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<ComplaintAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ComplaintAnnex> GetByComplaintIdIncludeAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.ComplaintAnnices.Include(x => x.ComplaintFollowUp.Complaint).Where(x => x.ComplaintFollowUp.ComplaintId.ToString() == id).FirstOrDefaultAsync(token);
            }
        }

        public async Task<List<ComplaintAnnex>> GetByComplaintIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.ComplaintAnnices.Include(x => x.ComplaintFollowUp.Complaint).Where(x => ids.Contains(x.ComplaintFollowUp.ComplaintId.ToString())).ToListAsync(token);
            }
        }

     

        public async Task<List<ComplaintAnnex>> GetByFollowUpIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.ComplaintAnnices.Where(x => ids.Contains(x.ComplaintFollowUpId.ToString())).ToListAsync(token);
            }
        }

        public Task<ComplaintAnnex> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintAnnex>> GetListAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintAnnex>> GetListIncludeAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }


        public Task UpdateAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
