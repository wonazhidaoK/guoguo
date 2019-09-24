using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOrderRepository : IPageIncludeRepository<Order, OrderDto, OrderForPageDto>
    {
        /// <summary>
        /// 提供给物业端的 订单列表 查询接口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<OrderForPageDto> GetAllIncludeForPropertyAsync(OrderDto dto, CancellationToken token = default);

        /// <summary>
        /// 提供给商家端的 订单列表 查询接口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<OrderForPageDto> GetAllIncludeForMerchantAsync(OrderDto dto, CancellationToken token = default);

        /// <summary>
        /// 更改交易状态接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Order> UpdatePaymentStatusAsync(string id, CancellationToken token = default);
    }
}
