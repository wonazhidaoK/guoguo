using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IIndustryService
    {
        Task<Industry> AddAsync(IndustryDto dto, CancellationToken token = default);

        Task UpdateAsync(IndustryDto dto, CancellationToken token = default);

        Task<List<Industry>> GetAllAsync(IndustryDto dto, CancellationToken token = default);

        Task DeleteAsync(IndustryDto dto, CancellationToken token = default);

        Task<Industry> GetAsync(string id, CancellationToken token = default);

        Task<List<Industry>> GetListAsync(IndustryDto dto, CancellationToken token = default);
    }
}
