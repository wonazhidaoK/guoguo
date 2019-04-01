using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public  interface IComplaintFollowUpRepository
    {
        Task<ComplaintFollowUp> AddAsync(ComplaintFollowUpDto dto, CancellationToken token = default);

        Task UpdateAsync(ComplaintFollowUpDto dto, CancellationToken token = default);

        Task<List<ComplaintFollowUp>> GetAllAsync(ComplaintFollowUpDto dto, CancellationToken token = default);

        Task DeleteAsync(ComplaintFollowUpDto dto, CancellationToken token = default);

        Task<ComplaintFollowUp> GetAsync(string id, CancellationToken token = default);

        Task<List<ComplaintFollowUp>> GetListAsync(ComplaintFollowUpDto dto, CancellationToken token = default);
    }
}
