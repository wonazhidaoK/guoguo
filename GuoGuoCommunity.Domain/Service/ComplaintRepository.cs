using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class ComplaintRepository : IComplaintRepository
    {
        public Task<Complaint> AddAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Complaint>> GetAllAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Complaint> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Complaint>> GetListAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ComplaintDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
