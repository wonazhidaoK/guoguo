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
    public class AnnouncementAnnexRepository : IAnnouncementAnnexRepository
    {
        public async Task<AnnouncementAnnex> AddAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.AnnouncementAnnices.Add(new AnnouncementAnnex
                {
                    AnnouncementId = dto.AnnouncementId,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,

                });
                if (!Guid.TryParse(entity.AnnexContent, out var annexContent))
                {
                    throw new NotImplementedException("公告附件id信息不正确！");
                }
                var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                entity.AnnexId = dto.AnnexContent;
                entity.AnnexContent = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;

                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<AnnouncementAnnex>> GetAllAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<AnnouncementAnnex> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.AnnouncementAnnices.Where(x => x.AnnouncementId == id).FirstOrDefaultAsync(token);
            }
        }

        public async Task<List<AnnouncementAnnex>> GetForAnnouncementIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.AnnouncementAnnices.Where(x => ids.Contains(x.AnnouncementId)).ToListAsync(token);
            }
        }

        public Task<List<AnnouncementAnnex>> GetListAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public string GetUrl(string id)
        {
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    var entity = db.AnnouncementAnnices.Where(x => x.AnnouncementId == id).FirstOrDefault();
                    if (!Guid.TryParse(entity.AnnexContent, out var annexContent))
                    {
                        throw new NotImplementedException("公告附件id信息不正确！");
                    }
                    var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                    return upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;
                }
            }
            catch (Exception)
            {
                return "";
            }

        }

        public Task UpdateAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
