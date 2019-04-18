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
    class VipOwnerCertificationAnnexRepository : IVipOwnerCertificationAnnexRepository
    {
        public async Task<VipOwnerCertificationAnnex> AddAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.CertificationConditionId, out var certificationConditionId))
                {
                    throw new NotImplementedException("高级认证申请条件id信息不正确！");
                }
                var vipOwnerCertification = await db.VipOwnerCertificationConditions.Where(x => x.Id == certificationConditionId).FirstOrDefaultAsync(token);
               
                var entity = db.VipOwnerCertificationAnnices.Add(new VipOwnerCertificationAnnex
                {
                    ApplicationRecordId = dto.ApplicationRecordId,
                    CertificationConditionId = dto.CertificationConditionId,
                    AnnexContent = dto.AnnexContent,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                });

                if (vipOwnerCertification.TypeValue == "Image")
                {
                    if (!Guid.TryParse(entity.AnnexContent, out var annexContent))
                    {
                        throw new NotImplementedException("高级认证附件id信息不正确！");
                    }
                    var upload = db.Uploads.Where(x => x.Id == annexContent).FirstOrDefault();
                    entity.AnnexId = dto.AnnexContent;
                    entity.AnnexContent = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File;
                }

                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VipOwnerCertificationAnnex>> GetAllAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<VipOwnerCertificationAnnex> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VipOwnerCertificationAnnex>> GetListAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.VipOwnerCertificationAnnices.Where(x =>  x.ApplicationRecordId == dto.ApplicationRecordId).ToListAsync(token);
            }
        }

        public string GetUrlAsync(string id, CancellationToken token = default)
        {
            try
            {
                using (var db = new GuoGuoCommunityContext())
                {
                    var entity = db.VipOwnerCertificationAnnices.Where(x => x.ApplicationRecordId == id).FirstOrDefault();
                    if (!Guid.TryParse(entity.AnnexContent, out var annexContent))
                    {
                        throw new NotImplementedException("高级认证附件id信息不正确！");
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

        public Task UpdateAsync(VipOwnerCertificationAnnexDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
