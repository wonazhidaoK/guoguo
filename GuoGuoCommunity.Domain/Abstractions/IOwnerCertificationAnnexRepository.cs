using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerCertificationAnnexRepository
    {
        Task<OwnerCertificationAnnex> AddAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task UpdateAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationAnnex>> GetAllAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task DeleteAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default);

        Task<OwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default);

        Task<List<OwnerCertificationAnnex>> GetListAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default);

        string GetUrl(string id);
        string GetPath(string id);
    }
}
