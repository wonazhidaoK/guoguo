using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Store;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShoppingTrolleyRepository : IIncludeRepository<ShoppingTrolley, ShoppingTrolleyDto>
    {
        Task<ShoppingTrolley> GetForShopCommodityIdAsync(ShoppingTrolleyDto dto, CancellationToken token = default);
    }
}
