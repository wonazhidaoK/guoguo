using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IAnnouncementAnnexRepository : IIncludeRepository<AnnouncementAnnex, AnnouncementAnnexDto>
    {
        /// <summary>
        /// 根据公告Id查询公告附件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<AnnouncementAnnex>> GetForAnnouncementIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
