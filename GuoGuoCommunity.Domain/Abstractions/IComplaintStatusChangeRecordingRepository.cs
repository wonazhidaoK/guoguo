using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IComplaintStatusChangeRecordingRepository
    {
        Task<ComplaintStatusChangeRecording> AddAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default);

        Task UpdateAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default);

        Task<List<ComplaintStatusChangeRecording>> GetAllAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default);

        Task DeleteAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default);

        Task<ComplaintStatusChangeRecording> GetAsync(string id, CancellationToken token = default);

        Task<List<ComplaintStatusChangeRecording>> GetListAsync(ComplaintStatusChangeRecordingDto dto, CancellationToken token = default);
    }
}
