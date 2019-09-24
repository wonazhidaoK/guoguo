using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Store;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShoppingTrolleyRepository : IIncludeRepository<ShoppingTrolley, ShoppingTrolleyDto>
    {
        /// <summary>
        /// 根据商户商品id 查询购物车内商品
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ShoppingTrolley> GetForShopCommodityIdAsync(ShoppingTrolleyDto dto, CancellationToken token = default);
    }
}
