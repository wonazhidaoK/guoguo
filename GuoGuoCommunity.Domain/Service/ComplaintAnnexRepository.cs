using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class ComplaintAnnexRepository : IComplaintAnnexRepository
    {
        public Task<ComplaintAnnex> AddAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintAnnex>> GetAllAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<ComplaintAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplaintAnnex>> GetListAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ComplaintAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
