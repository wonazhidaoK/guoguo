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
    class UploadRepository : IUploadService
    {
        public async Task<Upload> AddAsync(UploadDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.Uploads.Add(new Upload
                {
                    Agreement = dto.Agreement,
                    Directory = dto.Directory,
                    Domain = dto.Domain,
                    File = dto.File,
                    Host = dto.Id,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Upload>> GetAllAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Upload> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.Uploads.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该上传Id信息不正确！");
            }
        }

        public Task<List<Upload>> GetListAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
