using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerCertificationRecordRepository
    {
        Task<OwnerCertificationRecord> AddAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<OwnerCertificationRecord> UpdateAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateStatusAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetAllAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<OwnerCertificationRecord> GetAsync(string id, CancellationToken token = default);

        /// <summary>
        /// 当前登陆用户认证业户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<OwnerCertificationRecord>> GetListAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetAllForSmallDistrictIdAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetListForOwnerAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateInvalidAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据小区Id获取有效认证人列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<OwnerCertificationRecord>> GetListForSmallDistrictIdAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);
    }
}
