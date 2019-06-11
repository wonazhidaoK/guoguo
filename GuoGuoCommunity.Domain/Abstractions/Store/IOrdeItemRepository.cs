using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOrdeItemRepository : IIncludeRepository<OrderItem, OrdeItemDto>
    {
        Task<List<OrderItem>> GetListIncludeForOrderIdsAsync(List<string> ids, CancellationToken token = default);

        Task<List<OrderItem>> GetListIncludeForOrderIdAsync(string id, CancellationToken token = default);
    }
}
