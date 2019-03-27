﻿using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IVoteResultRecordRepository
    {
        Task<VoteResultRecord> AddAsync(VoteResultRecordDto dto, CancellationToken token = default);

        Task UpdateAsync(VoteResultRecordDto dto, CancellationToken token = default);

        Task<List<VoteResultRecord>> GetAllAsync(AnnouncementAnnexDto dto, CancellationToken token = default);

        Task DeleteAsync(VoteResultRecordDto dto, CancellationToken token = default);

        Task<VoteResultRecord> GetAsync(string id, CancellationToken token = default);
        
        Task<List<VoteResultRecord>> GetListAsync(VoteResultRecordDto dto, CancellationToken token = default);
    }
}
