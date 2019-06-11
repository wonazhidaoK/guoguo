using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IShopUserAddressRepository : IPageIncludeRepository<ShopUserAddress, ShopUserAddressDto, ShopUserAddressForPageDto>
    {
    }
}
