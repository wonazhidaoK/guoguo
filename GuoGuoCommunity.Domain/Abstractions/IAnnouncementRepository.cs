using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> AddAsync(AnnouncementDto dto, CancellationToken token = default);

        Task UpdateAsync(AnnouncementDto dto, CancellationToken token = default);

        Task<List<Announcement>> GetAllAsync(AnnouncementDto dto, CancellationToken token = default);

        Task DeleteAsync(AnnouncementDto dto, CancellationToken token = default);

        Task<Announcement> GetAsync(string id, CancellationToken token = default);

        Task<List<Announcement>> GetListForStreetOfficeAsync(AnnouncementDto dto, CancellationToken token = default);
        Task<List<Announcement>> GetListPropertyAsync(AnnouncementDto dto, CancellationToken token = default);
    }
}
