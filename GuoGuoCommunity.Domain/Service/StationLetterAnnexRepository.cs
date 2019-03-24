using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class StationLetterAnnexRepository : IStationLetterAnnexRepository
    {
        public async Task<StationLetterAnnex> AddAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.StationLetterAnnices.Add(new StationLetterAnnex
                {
                    StationLetterId = dto.StationLetterId,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
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

        public string GetUrl(string id)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.StationLetterAnnices.Where(x => x.StationLetterId == id).FirstOrDefault();
                var upload = db.Uploads.Where(x => x.Id == Guid.Parse(entity.AnnexContent)).FirstOrDefault();
                return upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;
            }
        }

        public Task UpdateAsync(StationLetterAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
