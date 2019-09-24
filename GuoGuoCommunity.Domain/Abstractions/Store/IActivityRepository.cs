using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Dto.Store;
using GuoGuoCommunity.Domain.Models.Store;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions.Store
{
    public interface IActivityRepository: IIncludeRepository<Activity, ActivityDto>
    {
        Task<List<Activity>> GetAllActivities(CancellationToken token = default);
    }
}
