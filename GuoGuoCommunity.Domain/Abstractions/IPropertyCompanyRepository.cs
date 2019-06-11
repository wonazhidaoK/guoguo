using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IPropertyCompanyRepository : IPageRepository<PropertyCompany, PropertyCompanyDto, PropertyCompanyForPageDto>
    {
    }
}
