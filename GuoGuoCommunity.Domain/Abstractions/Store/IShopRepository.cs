using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShopRepository : IIncludeRepository<Shop, ShopDto>
    {
        /// <summary>
        /// 更改商店信息  返回更改行数值
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        new Task<int> UpdateAsync(ShopDto dto, CancellationToken token = default);

        /// <summary>
        /// 查询小区内已拥有的小区商店Id 查询未添加到本小区的商店集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Shop>> GetListForNotIdsAsync(List<string> ids, CancellationToken token = default);

        Task<bool> UpdateShopActivitySign(ShopDto dto, CancellationToken token = default);
    }
}
