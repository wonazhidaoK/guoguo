using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStationLetterRepository
    {
        Task<StationLetter> AddAsync(StationLetterDto dto, CancellationToken token = default);

        Task UpdateAsync(StationLetterDto dto, CancellationToken token = default);

        Task<List<StationLetter>> GetAllAsync(StationLetterDto dto, CancellationToken token = default);

        Task DeleteAsync(StationLetterDto dto, CancellationToken token = default);

        Task<StationLetter> GetAsync(string id, CancellationToken token = default);

        Task<List<StationLetter>> GetListAsync(StationLetterDto dto, CancellationToken token = default);
    }
}
