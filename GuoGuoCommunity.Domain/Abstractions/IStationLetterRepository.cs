using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStationLetterRepository : IIncludeRepository<StationLetter, StationLetterDto>
    {
        /// <summary>
        /// 提供给物业端的查询列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<StationLetter>> GetAllForPropertyAsync(StationLetterDto dto, CancellationToken token = default);
    }
}
