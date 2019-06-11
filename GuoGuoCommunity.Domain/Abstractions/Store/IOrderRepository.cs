using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOrderRepository : IPageIncludeRepository<Order, OrderDto, OrderForPageDto>
    {
        Task<OrderForPageDto> GetAllIncludeForPropertyAsync(OrderDto dto, CancellationToken token = default);

        Task<OrderForPageDto> GetAllIncludeForMerchantAsync(OrderDto dto, CancellationToken token = default);
    }
}
