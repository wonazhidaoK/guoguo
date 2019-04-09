using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class StationLetterBrowseRecordRepository : IStationLetterBrowseRecordRepository
    {
        public async Task<StationLetterBrowseRecord> AddAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.StationLetterBrowseRecords.Add(new StationLetterBrowseRecord
                {
                    StationLetterId = dto.StationLetterId,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<StationLetterBrowseRecord>> GetAllAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<StationLetterBrowseRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StationLetterBrowseRecord>> GetListAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.StationLetterBrowseRecords.Where(x => x.StationLetterId == dto.StationLetterId && x.CreateOperationUserId == dto.OperationUserId).ToListAsync(token);
            }
        }

        public Task UpdateAsync(StationLetterBrowseRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
