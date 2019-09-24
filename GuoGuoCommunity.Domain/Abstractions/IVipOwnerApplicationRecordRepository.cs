using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVipOwnerApplicationRecordRepository : IIncludeRepository<VipOwnerApplicationRecord, VipOwnerApplicationRecordDto>
    {
        /// <summary>
        /// 查询小区内有效高级认证
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerApplicationRecord>> GetAllInvalidAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetAllForSmallDistrictIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<VipOwnerApplicationRecord> GetForVoteQuestionIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据投票问题Id获取业委会成员认证记录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VipOwnerApplicationRecord> GetForVoteQuestionOptionIdAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetListAsync(List<string> dto, CancellationToken token = default);

        /// <summary>
        /// 查询用户有效认证记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<VipOwnerApplicationRecord>> GetListAsync(string userId, CancellationToken token = default);

        Task<bool> IsPresenceforUserId(string userId, CancellationToken token = default);

        /// <summary>
        /// 更改通过状态
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Adopt(VipOwnerApplicationRecordDto dto, CancellationToken token = default);

        Task<List<VipOwnerApplicationRecord>> GetListAdoptAsync(List<string> dto, CancellationToken token = default);

        /// <summary>
        /// 更改申请认证信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task UpdateVoteAsync(VipOwnerApplicationRecordDto dto, CancellationToken token = default);
    }
}
