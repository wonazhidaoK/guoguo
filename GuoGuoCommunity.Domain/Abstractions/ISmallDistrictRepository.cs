using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ISmallDistrictRepository : IIncludeRepository<SmallDistrict, SmallDistrictDto>
    {
        /// <summary>
        /// 根据小区Id集合获取小区集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<SmallDistrict>> GetForIdsIncludeAsync(List<string> ids, CancellationToken token = default);
    }
}
