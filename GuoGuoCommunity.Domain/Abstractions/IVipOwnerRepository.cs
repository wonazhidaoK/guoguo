using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerRepository
    {
        Task<VipOwner> AddAsync(VipOwnerDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<List<VipOwner>> GetAllAsync(VipOwnerDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<VipOwner> GetAsync(string id, CancellationToken token = default);

        Task<List<VipOwner>> GetListAsync(VipOwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据街道办获取业委会
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwner>> GetListForStreetOfficeIdAsync(VipOwnerDto dto, CancellationToken token = default);

        Task<List<VipOwner>> GetIsValidAsync(VipOwnerDto dto, CancellationToken token = default);
    }
}
