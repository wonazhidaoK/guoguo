using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteQuestionOptionRepository : IIncludeRepository<VoteQuestionOption, VoteQuestionOptionDto>
    {
        /// <summary>
        /// 更改投票次数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<VoteQuestionOption> AddCountAsync(string id, CancellationToken token = default);
    }
}
