﻿using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IComplaintAnnexRepository : IIncludeRepository<ComplaintAnnex, ComplaintAnnexDto>
    {
        //Task<ComplaintAnnex> AddAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        Task<ComplaintAnnex> AddForFollowUpIdAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        //Task UpdateAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        //Task<List<ComplaintAnnex>> GetAllAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        //Task DeleteAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        Task<ComplaintAnnex> GetByComplaintIdIncludeAsync(string id, CancellationToken token = default);

        Task<List<ComplaintAnnex>> GetByComplaintIdsAsync(List<string> ids, CancellationToken token = default);

        Task<ComplaintAnnex> GetByFollowUpIdAsync(string id, CancellationToken token = default);

        Task<List<ComplaintAnnex>> GetByFollowUpIdsAsync(List<string> ids, CancellationToken token = default);

        //Task<List<ComplaintAnnex>> GetListAsync(ComplaintAnnexDto dto, CancellationToken token = default);

        string GetUrl(string id);

        string GetUrlForFollowUpId(string id);
    }
}
