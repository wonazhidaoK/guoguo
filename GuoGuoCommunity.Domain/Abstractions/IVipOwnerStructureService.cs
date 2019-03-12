using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerStructureService
    {
        Task<VipOwnerStructure> AddAsync(VipOwnerStructureDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerStructureDto dto, CancellationToken token = default);

        Task<List<VipOwnerStructure>> GetAllAsync(VipOwnerStructureDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerStructureDto dto, CancellationToken token = default);

        Task<VipOwnerStructure> GetAsync(string id, CancellationToken token = default);

        Task<List<VipOwnerStructure>> GetListAsync(CancellationToken token = default);
    }
}
