using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public  interface IVipOwnerCertificationConditionRepository : IIncludeRepository<VipOwnerCertificationCondition, VipOwnerCertificationConditionDto>
    {
        /// <summary>
        /// 获取所有认证申请条件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerCertificationCondition>> GetListAsync(CancellationToken token = default);
    }
}
