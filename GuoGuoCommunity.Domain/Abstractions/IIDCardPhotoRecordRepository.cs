using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IIDCardPhotoRecordRepository
    {
        Task<IDCardPhotoRecord> AddAsync(IDCardPhotoRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(IDCardPhotoRecordDto dto, CancellationToken token = default);

        Task<List<IDCardPhotoRecord>> GetAllAsync(IDCardPhotoRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(IDCardPhotoRecordDto dto, CancellationToken token = default);

        Task<IDCardPhotoRecord> GetAsync(string id, CancellationToken token = default);

        string GetUrl(string id);

        Task<List<IDCardPhotoRecord>> GetListAsync(IDCardPhotoRecordDto dto, CancellationToken token = default);
    }
}
