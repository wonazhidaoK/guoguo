using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllAsync(CancellationToken token = default);

        Task AddAsync(MenuDto dto, CancellationToken token = default);

        Task DeleteAsync(string id, CancellationToken token = default);

        Task UpdateAsync(MenuDto dto, CancellationToken token = default);

        Task<Menu> GetByIdAsync(string id, CancellationToken token = default);
    }
}
