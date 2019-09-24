using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IRoleRepository : IIncludeRepository<User_Role, RoleDto>
    {
    }
}
