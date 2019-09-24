using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IWeiXinUserRepository : IIncludeRepository<WeiXinUser, WeiXinUserDto>
    {
        /// <summary>
        /// 根据UnionId更改用微信用户信息(这里是取关事件调用，但是一直没有生效)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateForUnionIdAsync(WeiXinUserDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据UnionIds集合获取微信用户集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<WeiXinUser>> GetForUnionIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
