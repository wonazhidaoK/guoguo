using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IAnnouncementAnnexRepository
    {
        Task<AnnouncementAnnex> AddAsync(AnnouncementAnnexDto dto, CancellationToken token = default);

        Task UpdateAsync(AnnouncementAnnexDto dto, CancellationToken token = default);

        Task<List<AnnouncementAnnex>> GetAllAsync(AnnouncementAnnexDto dto, CancellationToken token = default);

        Task DeleteAsync(AnnouncementAnnexDto dto, CancellationToken token = default);

        Task<AnnouncementAnnex> GetAsync(string id, CancellationToken token = default);

        Task<List<AnnouncementAnnex>> GetForAnnouncementIdsAsync(List<string> ids, CancellationToken token = default);

        string GetUrl(string id);

        Task<List<AnnouncementAnnex>> GetListAsync(AnnouncementAnnexDto dto, CancellationToken token = default);
    }
}
