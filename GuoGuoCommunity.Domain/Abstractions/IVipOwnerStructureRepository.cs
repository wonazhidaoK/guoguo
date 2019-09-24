using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerStructureRepository : IIncludeRepository<VipOwnerStructure, VipOwnerStructureDto>
    {
        /// <summary>
        /// 查询所有业委会架构
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerStructure>> GetListAsync(CancellationToken token = default);
    }
}
