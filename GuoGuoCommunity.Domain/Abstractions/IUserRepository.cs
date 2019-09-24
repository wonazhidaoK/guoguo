using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IUserRepository : IIncludeRepository<User, UserDto>
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
        /// 获取商户账户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserPageDto> GetAllShopAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加街道办账号
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> AddStreetOfficeAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加物业账号
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> AddPropertyAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加商户用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> AddShopAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 添加微信用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> AddWeiXinAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 获取用户信息(会加载出外键关联对象)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> GetIncludeAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> GetForIdAsync(string id, CancellationToken token = default);

        /// <summary>
        /// 根据用户Id集合获取用户集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<User>> GetByIdsAsync(List<string> ids, CancellationToken token = default);

        /// <summary>
        /// 更新Token存储Token信息(没有实际应用，打算做同一账号只能一端登陆)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateTokenAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据小程序OpenId获取用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> GetForOpenIdAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据UnionId获取用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<User> GetForUnionIdAsync(UserDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据店铺Id获取用户集合
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<User>> GetByShopIdAsync(string shopId, CancellationToken token = default);
    }
}
