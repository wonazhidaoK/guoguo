using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ISmallDistrictRepository : IIncludeRepository<SmallDistrict, SmallDistrictDto>
    {
        Task<List<SmallDistrict>> GetForIdsIncludeAsync(List<string> ids, CancellationToken token = default);
    }
}
