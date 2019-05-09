using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class OwnerCertificationAnnexRepository : IOwnerCertificationAnnexRepository
    {
        public async Task<OwnerCertificationAnnex> AddAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(dto.ApplicationRecordId, out Guid applicationRecordId))
                {

                }
                var entity = db.OwnerCertificationAnnices.Add(new OwnerCertificationAnnex
                {
                    ApplicationRecordId = applicationRecordId,
                    OwnerCertificationAnnexTypeValue = dto.OwnerCertificationAnnexTypeValue,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
                if (!Guid.TryParse(dto.AnnexContent, out var annexContent))
                {
                    throw new NotImplementedException("业主认证附件id信息不正确！");
                }
                var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                entity.AnnexId = annexContent;
                entity.AnnexContent = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OwnerCertificationAnnex>> GetAllAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.OwnerCertificationAnnices.Where(x => x.ApplicationRecordId.ToString() == id).FirstOrDefaultAsync(token);
            }
        }

        public Task<List<OwnerCertificationAnnex>> GetListAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public string GetPath(string id)
        {
            //DirectoryInfo rootDir = Directory.GetParent(Environment.CurrentDirectory);
            //string root = rootDir.Parent.Parent.FullName;
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    if (!Guid.TryParse(id, out var uid))
                    {
                        throw new NotImplementedException("认证附件id信息不正确！");
                    }
                    var entity = db.OwnerCertificationAnnices.Where(x => x.Id == uid).FirstOrDefault();
                    //if (!Guid.TryParse(entity.AnnexId, out var annexId))
                    //{
                    //    throw new NotImplementedException("认证附件id信息不正确！");
                    //}
                    var upload = db.Uploads.Where(x => x.Id == entity.AnnexId).FirstOrDefault();
                    DirectoryInfo rootDir = Directory.GetParent(Environment.CurrentDirectory);
                    string root = rootDir.Parent.Parent.FullName;
                    return upload.Domain + "\\" + upload.Directory + "\\" + upload.File;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetUrl(string id)
        {
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    var entity = db.OwnerCertificationAnnices.Where(x => x.ApplicationRecordId.ToString() == id).FirstOrDefault();
                    if (!Guid.TryParse(entity.AnnexContent, out var annexContent))
                    {
                        throw new NotImplementedException("认证附件id信息不正确！");
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

        public Task UpdateAsync(OwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
