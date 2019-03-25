using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class VoteRecordRepository : IVoteRecordRepository
    {
        public Task<VoteRecord> AddAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecord>> GetAllAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecord>> GetListAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
