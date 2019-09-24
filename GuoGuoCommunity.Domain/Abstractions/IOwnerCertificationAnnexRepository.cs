using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerCertificationAnnexRepository : IIncludeRepository<OwnerCertificationAnnex, OwnerCertificationAnnexDto>
    {
        /// <summary>
        /// 根据附件Id查询附件地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetPath(string id);
    }
}
