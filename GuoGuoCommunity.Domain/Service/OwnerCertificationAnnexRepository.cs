using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
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
                var entity = db.OwnerCertificationAnnices.Add(new OwnerCertificationAnnex
                {
                    ApplicationRecordId = dto.ApplicationRecordId,
                    OwnerCertificationAnnexTypeValue = dto.OwnerCertificationAnnexTypeValue,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });
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

        public Task<OwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
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
                    var entity = db.OwnerCertificationAnnices.Where(x => x.ApplicationRecordId == id).FirstOrDefault();
                    if (!Guid.TryParse(entity.AnnexContent, out var annexContent))
                    {
                        throw new NotImplementedException("认证附件id信息不正确！");
                    }
                    var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                    DirectoryInfo rootDir = Directory.GetParent(Environment.CurrentDirectory);
                    string root = rootDir.Parent.Parent.FullName;
                    return  upload.Domain + "\\"+upload.Directory +"\\" +upload.File;
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
                    var entity = db.OwnerCertificationAnnices.Where(x => x.ApplicationRecordId == id).FirstOrDefault();
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
