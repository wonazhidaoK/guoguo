using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    interface IVipOwnerCertificationConditionRepository
    {
        Task<VipOwnerCertificationCondition> AddAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default);

        Task<List<VipOwnerCertificationCondition>> GetAllAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default);

        Task<VipOwnerCertificationCondition> GetAsync(string id, CancellationToken token = default);

        Task<List<VipOwnerCertificationCondition>> GetListAsync(VipOwnerCertificationConditionDto dto, CancellationToken token = default);
    }
}
