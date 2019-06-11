using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ISmallDistrictShopRepository : IPageIncludeRepository<SmallDistrictShop, SmallDistrictShopDto, SmallDistrictShopForPageDto>
    {
        Task<SmallDistrictShopForPageDto> GetAllIncludeForShopUserAsync(SmallDistrictShopDto dto, CancellationToken token = default);

        Task<SmallDistrictShop> GetIncludeForShopUserAsync(SmallDistrictShopDto dto, CancellationToken token = default);
    }
}
