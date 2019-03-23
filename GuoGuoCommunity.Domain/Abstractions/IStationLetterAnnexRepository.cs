using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStationLetterAnnexRepository
    {
        Task<StationLetterAnnex> AddAsync(StationLetterAnnexDto dto, CancellationToken token = default);

        Task UpdateAsync(StationLetterAnnexDto dto, CancellationToken token = default);

        Task<List<StationLetterAnnex>> GetAllAsync(StationLetterAnnexDto dto, CancellationToken token = default);

        Task DeleteAsync(StationLetterAnnexDto dto, CancellationToken token = default);

        Task<StationLetterAnnex> GetAsync(string id, CancellationToken token = default);

        Task<List<StationLetterAnnex>> GetListAsync(StationLetterAnnexDto dto, CancellationToken token = default);
    }
}
