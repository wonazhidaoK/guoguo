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
        public Task<ComplaintStatusChangeRecording> AddAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
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
