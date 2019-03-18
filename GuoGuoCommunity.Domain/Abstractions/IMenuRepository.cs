using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllAsync(CancellationToken token = default);

        Task<Menu> AddAsync(MenuDto dto, CancellationToken token = default);

        Task DeleteAsync(MenuDto dto, CancellationToken token = default);

        Task UpdateAsync(MenuDto dto, CancellationToken token = default);

        Task<Menu> GetByIdAsync(string id, CancellationToken token = default);
    }
}
