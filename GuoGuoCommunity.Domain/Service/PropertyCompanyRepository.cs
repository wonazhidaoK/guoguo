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
    public class PropertyCompanyRepository : IPropertyCompanyRepository
    {
        public async Task<PropertyCompany> AddAsync(PropertyCompanyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if ((await db.PropertyCompanies.Where(x => x.Name == dto.Name && x.IsDeleted == false).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该物业公司名称已存在！");
                }
                var entity = db.PropertyCompanies.Add(new PropertyCompany
                {
                    Name = dto.Name,
                    LogoImageUrl = dto.LogoImageUrl,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    Description = dto.Description,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(PropertyCompanyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("物业公司Id信息不正确！");
                }
                var propertyCompany = await db.PropertyCompanies.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (propertyCompany == null)
                {
                    throw new NotImplementedException("该物业公司不存在！");
                }
                if (await OnDelete(db, dto, token))
                {
                    throw new NotImplementedException("该物业公司存存在关联数据");
                }
                propertyCompany.LastOperationTime = dto.OperationTime;
                propertyCompany.LastOperationUserId = dto.OperationUserId;
                propertyCompany.DeletedTime = dto.OperationTime;
                propertyCompany.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public Task<List<PropertyCompany>> GetAllAsync(PropertyCompanyDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PropertyCompany> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (Guid.TryParse(id, out var uid))
                {
                    return await db.PropertyCompanies.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                }
                throw new NotImplementedException("该物业公司Id信息不正确！");
            }
        }

        public async Task<List<PropertyCompany>> GetListAsync(PropertyCompanyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.PropertyCompanies.Where(item => item.IsDeleted == false);

                return await list.ToListAsync(token);
            }
        }

        public async Task<PropertyCompanyForPageDto> GetListForPageAsync(PropertyCompanyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = db.PropertyCompanies.Where(item => item.IsDeleted == false);
                if (!string.IsNullOrEmpty(dto.Name))
                {
                    list = list.Where(item => item.Name.Contains(dto.Name));
                }
                if (!string.IsNullOrEmpty(dto.Phone))
                {
                    list = list.Where(item => item.Phone.Contains(dto.Phone));
                }

                list = list.OrderByDescending(item => item.CreateOperationTime);
                List<PropertyCompany> resultList = await list.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToListAsync(token);
                PropertyCompanyForPageDto pagelist = new PropertyCompanyForPageDto { List = resultList, Count = list.Count() };
                return pagelist;
            }
        }

        public async Task UpdateAsync(PropertyCompanyDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("物业公司Id信息不正确！");
                }
                var propertyCompany = await db.PropertyCompanies.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (propertyCompany == null)
                {
                    throw new NotImplementedException("该物业公司不存在！");
                }
                if ((await db.PropertyCompanies.Where(x => x.Name == dto.Name && x.IsDeleted == false && x.Id != uid).FirstOrDefaultAsync(token)) != null)
                {
                    throw new NotImplementedException("该物业公司名称已存在！");
                }
                propertyCompany.Name = dto.Name;
                propertyCompany.Phone = dto.Phone;
                propertyCompany.Address = dto.Address;
                propertyCompany.Description = dto.Description;
                propertyCompany.LogoImageUrl = dto.LogoImageUrl;
                propertyCompany.LastOperationTime = dto.OperationTime;
                propertyCompany.LastOperationUserId = dto.OperationUserId;
                if (await db.SaveChangesAsync(token) <= 0)
                    throw new NotImplementedException("数据执行失败。");
            }
        }

        private async Task<bool> OnDelete(GuoGuoCommunityContext db, PropertyCompanyDto dto, CancellationToken token = default)
        {
            //小区信息
            if (await db.SmallDistricts.Where(x => x.PropertyCompanyId.ToString() == dto.Id && x.IsDeleted == false).FirstOrDefaultAsync(token) != null)
            {
                return true;
            }

            return false;
        }
    }
}
