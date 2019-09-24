using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IAnnouncementRepository : IIncludeRepository<Announcement, AnnouncementDto>
    {
        /// <summary>
        /// 业委会添加公告信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Announcement> AddVipOwnerAsync(AnnouncementDto dto, CancellationToken token = default);

        /// <summary>
        /// 业委会公告列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Announcement>> GetAllForVipOwnerAsync(AnnouncementDto dto, CancellationToken token = default);

        /// <summary>
        /// 街道办公告列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Announcement>> GetListForStreetOfficeAsync(AnnouncementDto dto, CancellationToken token = default);

        /// <summary>
        /// 物业公告列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Announcement>> GetListPropertyAsync(AnnouncementDto dto, CancellationToken token = default);
    }
}
