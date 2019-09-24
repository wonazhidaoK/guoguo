using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ISmallDistrictShopRepository : IPageIncludeRepository<SmallDistrictShop, SmallDistrictShopDto, SmallDistrictShopForPageDto>
    {
        /// <summary>
        /// 根据小区Id查询 小区内的商店数据
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<SmallDistrictShopForPageDto> GetAllIncludeForShopUserAsync(SmallDistrictShopDto dto, CancellationToken token = default);
    }
}
