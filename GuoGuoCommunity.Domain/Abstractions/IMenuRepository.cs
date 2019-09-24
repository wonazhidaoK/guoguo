using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IMenuRepository : IIncludeRepository<Menu, MenuDto>
    {
        Task<Menu> GetByIdAsync(string id, CancellationToken token = default);

        /// <summary>
        /// 根据菜单Id集合查询菜单集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Menu>> GetByIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
