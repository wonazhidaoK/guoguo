using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class VoteRecordDetailRepository : IVoteRecordDetailRepository
    {
        public Task<VoteRecordDetail> AddAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecordDetail>> GetAllAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VoteRecordDetail> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoteRecordDetail>> GetListAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(VoteRecordDetailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
