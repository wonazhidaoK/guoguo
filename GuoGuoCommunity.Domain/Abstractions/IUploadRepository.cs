using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
   public interface IUploadRepository
    {
        Task<Upload> AddAsync(UploadDto dto, CancellationToken token = default);

        Task UpdateAsync(UploadDto dto, CancellationToken token = default);

        Task<List<Upload>> GetAllAsync(UploadDto dto, CancellationToken token = default);

        Task DeleteAsync(UploadDto dto, CancellationToken token = default);

        Task<Upload> GetAsync(string id, CancellationToken token = default);

        Task<List<Upload>> GetListAsync(UploadDto dto, CancellationToken token = default);
    }
}
