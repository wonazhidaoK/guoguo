using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteAnnexRepository : IIncludeRepository<VoteAnnex, VoteAnnexDto>
    {
        /// <summary>
        /// 根据附件Id查询url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetUrl(string id);
    }
}
