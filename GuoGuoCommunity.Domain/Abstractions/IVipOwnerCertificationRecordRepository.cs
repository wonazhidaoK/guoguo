using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerCertificationRecordRepository : IIncludeRepository<VipOwnerCertificationRecord, VipOwnerCertificationRecordDto>
    {
        /// <summary>
        /// 提供给物业 查询列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerCertificationRecord>> GetAllForPropertyAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据业委会id查询高级认证
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VipOwnerCertificationRecord> GetForVipOwnerIdAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据业委会id集合查询高级认证
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerCertificationRecord>> GetForVipOwnerIdsAsync(List<string> ids, CancellationToken token = default);
    }
}
