using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IRoleMenuRepository
    {
        Task<List<Role_Menu>> GetAllAsync(CancellationToken token = default);

        Task AddAsync(RoleMenuDto dto, CancellationToken token = default);

        Task DeleteAsync(string id, CancellationToken token = default);

        Task UpdateAsync(RoleMenuDto dto, CancellationToken token = default);

        Task<List<Role_Menu>> GetByRoleIdAsync(string roleId,CancellationToken token =default);

    }
}
