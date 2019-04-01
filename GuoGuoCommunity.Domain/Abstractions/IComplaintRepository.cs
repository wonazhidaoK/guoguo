using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public  interface IComplaintRepository
    {
        Task<Complaint> AddAsync(ComplaintDto dto, CancellationToken token = default);

        Task UpdateAsync(ComplaintDto dto, CancellationToken token = default);

        Task<List<Complaint>> GetAllAsync(ComplaintDto dto, CancellationToken token = default);

        Task DeleteAsync(ComplaintDto dto, CancellationToken token = default);

        Task<Complaint> GetAsync(string id, CancellationToken token = default);

        Task<List<Complaint>> GetListAsync(ComplaintDto dto, CancellationToken token = default);
    }
}
