using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class ComplaintFollowUpRepository : IComplaintFollowUpRepository
    {
        public Task<ComplaintFollowUp> AddAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintFollowUp>> GetAllAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<ComplaintFollowUp> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintFollowUp>> GetListAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ComplaintFollowUpDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
