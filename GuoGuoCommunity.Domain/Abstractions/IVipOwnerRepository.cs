using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerRepository : IIncludeRepository<VipOwner, VipOwnerDto>
    {

        Task UpdateIsElectionAsync(VipOwnerDto dto, CancellationToken token = default);

        Task UpdateValidAsync(VipOwnerDto dto, CancellationToken token = default);

        Task UpdateInvalidAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<List<VipOwner>> GetListForPropertyAsync(VipOwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据街道办获取业委会
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwner>> GetListForStreetOfficeIdAsync(VipOwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据小区获取有效业委会
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VipOwner> GetForSmallDistrictIdAsync(VipOwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据小区id集合获取有效业委会
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwner>> GetForSmallDistrictIdsAsync(List<string> ids, CancellationToken token = default);

        /// <summary>
        /// 根据小区查询业委会记录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwner>> GetIsValidAsync(VipOwnerDto dto, CancellationToken token = default);
    }
}
