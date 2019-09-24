using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IRoleMenuRepository : IIncludeRepository<Role_Menu, RoleMenuDto>
    {
        /// <summary>
        /// 根据角色获取角色菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Role_Menu>> GetByRoleIdAsync(string roleId, CancellationToken token = default);
    }
}
