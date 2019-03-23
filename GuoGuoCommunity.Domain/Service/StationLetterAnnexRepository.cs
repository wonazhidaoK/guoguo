using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class StationLetterAnnexRepository : IStationLetterAnnexRepository
    {
        public Task<StationLetterAnnex> AddAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<StationLetterAnnex>> GetAllAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<StationLetterAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<StationLetterAnnex>> GetListAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
