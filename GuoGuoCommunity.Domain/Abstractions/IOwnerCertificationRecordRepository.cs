using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerCertificationRecordRepository : IIncludeRepository<OwnerCertificationRecord, OwnerCertificationRecordDto>
    {
        Task<List<OwnerCertificationRecord>> GetAllForSmallDistrictIdIncludeAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateInvalidAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据小区Id获取有效认证人列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<OwnerCertificationRecord>> GetListForSmallDistrictIdIncludeAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetListForSmallDistrictIdAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据业主认证id集合获取认证集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<OwnerCertificationRecord>> GetListForIdArrayIncludeAsync(List<string> ids, CancellationToken token = default);

        Task<List<OwnerCertificationRecord>> GetListForIdArrayAsync(List<string> ids, CancellationToken token = default);

        Task<OwnerCertificationRecord> UpdateStatusAsync(OwnerCertificationRecordDto dto, CancellationToken token = default);
    }
}
