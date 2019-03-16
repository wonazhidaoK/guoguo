using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(CancellationToken token = default);

        Task AddAsync(UserDto dto, CancellationToken token = default);

        Task DeleteAsync(string id, CancellationToken token = default);

        Task UpdateAsync(UserDto dto, CancellationToken token = default);

        Task<User> GetAsync(UserDto dto, CancellationToken token = default);

        Task UpdateTokenAsync(UserDto dto, CancellationToken token = default);

        Task<User> AddWeiXinAsync(UserDto dto, CancellationToken token = default);

        Task<User> GetForOpenIdAsync(UserDto dto, CancellationToken token = default);

        Task<User> GetForUnionIdAsync(UserDto dto, CancellationToken token = default);
    }
}
