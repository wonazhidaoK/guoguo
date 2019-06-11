using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IIndustryRepository : IIncludeRepository<Industry, IndustryDto>
    {
        Task<List<Industry>> GetForIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
