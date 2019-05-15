﻿using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public  interface IComplaintRepository : IIncludeRepository<Complaint, ComplaintDto>
    {
        //Task<Complaint> AddAsync(ComplaintDto dto, CancellationToken token = default);

        //Task UpdateAsync(ComplaintDto dto, CancellationToken token = default);

        Task UpdateForAppealAsync(ComplaintDto dto, CancellationToken token = default);

        Task UpdateForFinishedAsync(ComplaintDto dto, CancellationToken token = default);

        Task UpdateForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default);

        //Task<List<Complaint>> GetAllAsync(ComplaintDto dto, CancellationToken token = default);

        Task<List<Complaint>> GetAllForVipOwnerAsync(ComplaintDto dto, CancellationToken token = default);

        Task<List<Complaint>> GetAllForPropertyIncludeAsync(ComplaintDto dto, CancellationToken token = default);

        Task<List<Complaint>> GetAllForStreetOfficeIncludeAsync(ComplaintDto dto, CancellationToken token = default);

       // Task DeleteAsync(ComplaintDto dto, CancellationToken token = default);

       // Task<Complaint> GetAsync(string id, CancellationToken token = default);

       // Task<List<Complaint>> GetListAsync(ComplaintDto dto, CancellationToken token = default);

        Task ClosedAsync(ComplaintDto dto, CancellationToken token = default);

        Task ViewForVipOwnerAsync(ComplaintDto dto, CancellationToken token = default);

        Task ViewForPropertyAsync(ComplaintDto dto, CancellationToken token = default);

        Task ViewForStreetOfficeAsync(ComplaintDto dto, CancellationToken token = default);

        Task InvalidAsync(ComplaintDto dto, CancellationToken token = default);
    }
}
