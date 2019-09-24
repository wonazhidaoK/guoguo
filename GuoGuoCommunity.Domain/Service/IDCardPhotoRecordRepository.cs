using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class IDCardPhotoRecordRepository : IIDCardPhotoRecordRepository
    {
        public async Task<IDCardPhotoRecord> AddAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.IDCardPhotoRecords.Add(new IDCardPhotoRecord
                {
                    ApplicationRecordId = dto.ApplicationRecordId,
                    Message = dto.Message,
                    OwnerCertificationAnnexId = dto.OwnerCertificationAnnexId,
                    PhotoBase64 = dto.PhotoBase64,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDCardPhotoRecord>> GetAllAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDCardPhotoRecord>> GetAllIncludeAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IDCardPhotoRecord> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IDCardPhotoRecord> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDCardPhotoRecord>> GetListAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDCardPhotoRecord>> GetListIncludeAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IDCardPhotoRecordDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
