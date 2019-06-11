using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShopCommodityRepository : IPageIncludeRepository<ShopCommodity, ShopCommodityDto, ShopCommodityForPageDto>
    {
        Task UpdateSalesTypeAsync(ShopCommodityDto dto, CancellationToken token = default);
    }
}
