using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class ComplaintStatusChangeRecordingRepository : IComplaintStatusChangeRecordingRepository
    {
        public async Task<ComplaintStatusChangeRecording> AddAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.StatusChangeRecordings.Add(new ComplaintStatusChangeRecording
                {
                    ComplaintFollowUpId =Guid.Parse(dto.ComplaintFollowUpId) ,
                    //ComplaintId = dto.ComplaintId,
                    NewStatus = dto.NewStatus,
                    OldStatus = dto.OldStatus,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintStatusChangeRecording>> GetAllAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<ComplaintStatusChangeRecording> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintStatusChangeRecording>> GetListAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
