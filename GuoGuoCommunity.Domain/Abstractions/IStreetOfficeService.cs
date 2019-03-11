using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStreetOfficeService
    {
        Task<List<StreetOffice>> GetAllAsync(StreetOfficeDto dto, CancellationToken token = default);

        Task<StreetOffice> AddAsync(StreetOfficeDto dto, CancellationToken token = default);

        Task DeleteAsync(StreetOfficeDto dto, CancellationToken token = default);

        Task UpdateAsync(StreetOfficeDto dto, CancellationToken token = default);

        Task<StreetOffice> GetAsync(string id, CancellationToken token = default);

        Task<List<StreetOffice>> GetListAsync(StreetOfficeDto dto, CancellationToken token = default);
    }
}
