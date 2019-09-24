using EntityFramework.Extensions;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class StationLetterRepository : IStationLetterRepository
    {
        public async Task<StationLetter> AddAsync(StationLetterDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var entity = db.StationLetters.Add(new StationLetter
                {
                    Content = dto.Content,
                    SmallDistrictArray = dto.SmallDistrictArray,
                    StreetOfficeId = dto.StreetOfficeId,
                    StreetOfficeName = dto.StreetOfficeName,
                    Summary = dto.Summary,
                    Title = dto.Title,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public Task DeleteAsync(StationLetterDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StationLetter>> GetAllAsync(StationLetterDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.StationLetters.Where(x => x.IsDeleted == false && x.StreetOfficeId == dto.StreetOfficeId).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictArray))
                {
                    list = list.Where(x => x.SmallDistrictArray.Split(',').Contains(dto.SmallDistrictArray)).ToList();
                }
                list = list.Where(x => x.CreateOperationTime >= dto.ReleaseTimeStart && x.CreateOperationTime <= dto.ReleaseTimeEnd).ToList();
                return list;
            }
        }

        public async Task<StationLetter> GetAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(id, out var stationLetterId))
                {
                    throw new NotImplementedException("站内信Id信息不正确！");
                }

                return await db.StationLetters.Where(x => x.IsDeleted == false && x.Id == stationLetterId).FirstOrDefaultAsync(token);
            }
        }

        public async Task<List<StationLetter>> GetListAsync(StationLetterDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.StationLetters.Where(x => x.IsDeleted == false).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictArray))
                {
                    list = list.Where(x => x.SmallDistrictArray.Split(',').Contains(dto.SmallDistrictArray)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.ReadStatus))
                {
                    var letterIdList = await db.StationLetterBrowseRecords.Where(x => x.CreateOperationUserId == dto.OperationUserId).Select(x => x.StationLetterId).ToListAsync(token);
                    if (dto.ReadStatus == StationLetterReadStatus.HaveRead.Value)
                    {
                        list = list.Where(x => letterIdList.Contains(x.Id.ToString())).ToList();
                    }
                    else if (dto.ReadStatus == StationLetterReadStatus.UnRead.Value)
                    {
                        list = (from x in list where !letterIdList.Contains(x.Id.ToString()) select x).ToList(); //.Where(x => letterIdList.Contains(x.Id.ToString())).ToList();
                    }
                }
                return list;
            }
        }

        public Task UpdateAsync(StationLetterDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }


        public void OnSubscribe(StreetOfficeIncrementer incrementer)
        {
            incrementer.StreetOfficeEvent += StreetOfficeChanging;//在发布者私有委托里增加方法
        }

        public async void StreetOfficeChanging(GuoGuoCommunityContext dbs, StreetOffice streetOffice, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                await db.StationLetters.Where(x => x.StreetOfficeId == streetOffice.Id.ToString()).UpdateAsync(x => new StationLetter { StreetOfficeName = streetOffice.Name });
            }
        }

        public async Task<List<StationLetter>> GetAllForPropertyAsync(StationLetterDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.StationLetters.Where(x => x.IsDeleted == false).ToListAsync(token);

                if (!string.IsNullOrWhiteSpace(dto.SmallDistrictArray))
                {
                    list = list.Where(x => x.SmallDistrictArray.Split(',').Contains(dto.SmallDistrictArray)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.ReadStatus))
                {
                    var letterIdList = await db.StationLetterBrowseRecords.Where(x => x.CreateOperationUserId == dto.OperationUserId).Select(x => x.StationLetterId).ToListAsync(token);
                    if (dto.ReadStatus == StationLetterReadStatus.HaveRead.Value)
                    {
                        list = list.Where(x => letterIdList.Contains(x.Id.ToString())).ToList();
                    }
                    else if (dto.ReadStatus == StationLetterReadStatus.UnRead.Value)
                    {
                        list = (from x in list where !letterIdList.Contains(x.Id.ToString()) select x).ToList(); //.Where(x => letterIdList.Contains(x.Id.ToString())).ToList();
                    }
                }
                list = list.Where(x => x.CreateOperationTime >= dto.ReleaseTimeStart && x.CreateOperationTime <= dto.ReleaseTimeEnd).ToList();
                return list;
            }
        }

        public Task<List<StationLetter>> GetAllIncludeAsync(StationLetterDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<StationLetter> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<StationLetter>> GetListIncludeAsync(StationLetterDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
