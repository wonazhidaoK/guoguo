using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOrdeItemRepository : IIncludeRepository<OrderItem, OrdeItemDto>
    {
        /// <summary>
        /// 根据订单id列表查找 订单项
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<OrderItem>> GetListIncludeForOrderIdsAsync(List<string> ids, CancellationToken token = default);

        /// <summary>
        /// 根据订单id查找 订单项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<OrderItem>> GetListIncludeForOrderIdAsync(string id, CancellationToken token = default);
    }
}
