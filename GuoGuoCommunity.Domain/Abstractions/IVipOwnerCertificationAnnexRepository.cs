using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerCertificationAnnexRepository
    {
        Task<VipOwnerCertificationAnnex> AddAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task<List<VipOwnerCertificationAnnex>> GetAllAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task<VipOwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default);

        Task<string> GetUrlAsync(string id, CancellationToken token = default);

        Task<List<VipOwnerCertificationAnnex>> GetListAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default);
    }
}
