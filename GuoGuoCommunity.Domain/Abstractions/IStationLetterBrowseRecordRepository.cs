using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IStationLetterBrowseRecordRepository : IIncludeRepository<StationLetterBrowseRecord, StationLetterBrowseRecordDto>
    {
        /// <summary>
        /// 根据站内信Id获取站内信浏览记录
        /// </summary>
        /// <param name="stationLetterIds"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<StationLetterBrowseRecord>> GetForStationLetterIdsAsync(List<string> stationLetterIds, CancellationToken token = default);
    }
}
