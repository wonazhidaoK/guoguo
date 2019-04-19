using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerCertificationRecordRepository
    {
        Task<VipOwnerCertificationRecord> AddAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerCertificationRecord>> GetAllAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task DeleteAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        Task<VipOwnerCertificationRecord> GetAsync(string id, CancellationToken token = default);

        /// <summary>
        /// 根据用户Id获取高级认证记录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerCertificationRecord>> GetListAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据业委会id查询高级认证
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VipOwnerCertificationRecord> GetForVipOwnerIdAsync(VipOwnerCertificationRecordDto dto, CancellationToken token = default);
    }
}
