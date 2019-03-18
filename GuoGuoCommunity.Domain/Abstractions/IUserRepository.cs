using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IUserRepository
    {
        /// <summary>
        /// 获取物业账户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<User>> GetAllPropertyAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 获取街道账户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<User>> GetAllStreetOfficeAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加街道办账号
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddStreetOfficeAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加物业账号
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddPropertyAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加微信用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> AddWeiXinAsync(UserDto dto, CancellationToken token = default);

        Task DeleteAsync(string id, CancellationToken token = default);

        Task UpdateAsync(UserDto dto, CancellationToken token = default);

        Task<User> GetAsync(UserDto dto, CancellationToken token = default);

        Task UpdateTokenAsync(UserDto dto, CancellationToken token = default);

        Task<User> GetForOpenIdAsync(UserDto dto, CancellationToken token = default);

        Task<User> GetForUnionIdAsync(UserDto dto, CancellationToken token = default);
    }
}
