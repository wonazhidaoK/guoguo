using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IWeiXinUserRepository
    {
        Task<WeiXinUser> AddAsync(WeiXinUserDto dto, CancellationToken token = default);

        Task UpdateAsync(WeiXinUserDto dto, CancellationToken token = default);

        Task UpdateForUnionIdAsync(WeiXinUserDto dto, CancellationToken token = default);

        Task<List<WeiXinUser>> GetAllAsync(WeiXinUserDto dto, CancellationToken token = default);

        Task DeleteAsync(WeiXinUserDto dto, CancellationToken token = default);

        Task<WeiXinUser> GetAsync(string unionid, CancellationToken token = default);

        Task<List<WeiXinUser>> GetListAsync(WeiXinUserDto dto, CancellationToken token = default);
    }
}
