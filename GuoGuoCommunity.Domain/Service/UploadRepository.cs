using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    class UploadRepository : IUploadService
    {
        public Task<Upload> AddAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Upload>> GetAllAsync(UploadDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Upload> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
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
