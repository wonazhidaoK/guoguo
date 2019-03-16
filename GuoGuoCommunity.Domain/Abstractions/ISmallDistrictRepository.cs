using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ISmallDistrictRepository
    {
        Task<SmallDistrict> AddAsync(SmallDistrictDto dto, CancellationToken token = default);

        Task UpdateAsync(SmallDistrictDto dto, CancellationToken token = default);

        Task<List<SmallDistrict>> GetAllAsync(SmallDistrictDto dto, CancellationToken token = default);

        Task DeleteAsync(SmallDistrictDto dto, CancellationToken token = default);

        Task<SmallDistrict> GetAsync(string id, CancellationToken token = default);

        Task<List<SmallDistrict>> GetListAsync(SmallDistrictDto dto, CancellationToken token = default);
    }
}
