using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShopCommodityRepository : IPageIncludeRepository<ShopCommodity, ShopCommodityDto, ShopCommodityForPageDto>
    {
        /// <summary>
        /// 更改商品上下架状态接口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateSalesTypeAsync(ShopCommodityDto dto, CancellationToken token = default);
    }
}
