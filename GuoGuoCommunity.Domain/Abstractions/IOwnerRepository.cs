using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerRepository
    {
        Task<Owner> AddAsync(OwnerDto dto, CancellationToken token = default);

        Task UpdateAsync(OwnerDto dto, CancellationToken token = default);

        Task UpdateForLegalizeAsync(OwnerDto dto, CancellationToken token = default);

        Task<List<Owner>> GetAllAsync(OwnerDto dto, CancellationToken token = default);

        Task DeleteAsync(OwnerDto dto, CancellationToken token = default);

        Task<Owner> GetAsync(string id, CancellationToken token = default);

        Task<List<Owner>> GetListAsync(OwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 获取业户下未认证业主信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Owner>> GetListForLegalizeAsync(OwnerDto dto, CancellationToken token = default);

        Task<Owner> GetForOwnerCertificationRecordIdAsync(OwnerDto dto, CancellationToken token = default);
    }
}
