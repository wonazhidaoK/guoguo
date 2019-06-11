using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IGoodsTypeRepository : IPageIncludeRepository<GoodsType, GoodsTypeDto, GoodsTypeForPageDto>
    {
    }
}
