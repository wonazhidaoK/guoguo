using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IComplaintTypeRepository
    {
        Task<ComplaintType> AddAsync(ComplaintTypeDto dto, CancellationToken token = default);

        Task UpdateAsync(ComplaintTypeDto dto, CancellationToken token = default);

        Task<List<ComplaintType>> GetAllAsync(ComplaintTypeDto dto, CancellationToken token = default);

        Task DeleteAsync(ComplaintTypeDto dto, CancellationToken token = default);

        Task<ComplaintType> GetAsync(string id, CancellationToken token = default);

        Task<List<ComplaintType>> GetListAsync(ComplaintTypeDto dto, CancellationToken token = default);
    }
}
