using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IIndustryRepository : IIncludeRepository<Industry, IndustryDto>
    {
        /// <summary>
        /// 根据业户id集合 查询业户集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Industry>> GetForIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
