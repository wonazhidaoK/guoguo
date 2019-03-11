using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IRoleService
    {
        Task<List<User_Role>> GetAllAsync(CancellationToken token = default);

        Task AddAsync(RoleDto dto, CancellationToken token = default);

        Task DeleteAsync(string id, CancellationToken token = default);

        Task UpdateAsync(RoleDto dto, CancellationToken token = default);
    }
}
