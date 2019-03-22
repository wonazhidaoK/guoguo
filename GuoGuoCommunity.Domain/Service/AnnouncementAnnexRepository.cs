﻿using GuoGuoCommunity.Domain.Abstractions;
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

        public Task<List<AnnouncementAnnex>> GetListAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public string GetUrl(string id)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.AnnouncementAnnices.Where(x => x.AnnouncementId == id).FirstOrDefault();
                var upload = db.Uploads.Where(x => x.Id == Guid.Parse(entity.AnnexContent)).FirstOrDefault();
                return upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;
            }
        }

        public Task UpdateAsync(AnnouncementAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
