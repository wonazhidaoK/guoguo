using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IRoleRepository
    {
        Task<List<User_Role>> GetAllAsync(RoleDto dto, CancellationToken token = default);

        Task<User_Role> AddAsync(RoleDto dto, CancellationToken token = default);

        Task DeleteAsync(RoleDto dto, CancellationToken token = default);

        Task UpdateAsync(RoleDto dto, CancellationToken token = default);
    }
}
