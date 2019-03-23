using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStationLetterBrowseRecordRepository
    {
        Task<StationLetterBrowseRecord> AddAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default);

        Task<List<StationLetterBrowseRecord>> GetAllAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default);

        Task<StationLetterBrowseRecord> GetAsync(string id, CancellationToken token = default);

        Task<List<StationLetterBrowseRecord>> GetListAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default);
    }
}
