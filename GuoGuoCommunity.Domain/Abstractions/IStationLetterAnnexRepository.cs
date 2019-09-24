using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStationLetterAnnexRepository : IIncludeRepository<StationLetterAnnex, StationLetterAnnexDto>
    {
        /// <summary>
        /// 根据附件Id获取附件url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetUrl(string id);
    }
}
