using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteAssociationVipOwnerRepository : IIncludeRepository<VoteAssociationVipOwner, VoteAssociationVipOwnerDto>
    {
        /// <summary>
        /// 根据投票id查询业委关联投票信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VoteAssociationVipOwner> GetForVoteIdAsync(string id, CancellationToken token = default);
    }
}
