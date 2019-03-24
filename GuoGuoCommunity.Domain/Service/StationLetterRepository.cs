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
                return list;
            }
        }

        public Task UpdateAsync(StationLetterDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}