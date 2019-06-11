using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShopRepository : IIncludeRepository<Shop, ShopDto>
    {
        new Task<int> UpdateAsync(ShopDto dto, CancellationToken token = default);

        Task<List<Shop>> GetListForNotIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
