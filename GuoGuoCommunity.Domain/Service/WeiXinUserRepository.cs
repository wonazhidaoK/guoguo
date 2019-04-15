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
    public class WeiXinUserRepository : IWeiXinUserRepository
    {
        public async Task<WeiXinUser> AddAsync(WeiXinUserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var weiXinUser = await db.WeiXinUsers.Where(x => x.Unionid == dto.Unionid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (weiXinUser != null)
                {
                    weiXinUser.City = dto.City;
                    weiXinUser.Country = dto.Country;
                    weiXinUser.Groupid = dto.Groupid;
                    weiXinUser.Headimgurl = dto.Headimgurl;
                    weiXinUser.Language = dto.Language;
                    weiXinUser.Nickname = dto.Nickname;
                    weiXinUser.OpenId = dto.Openid;
                    weiXinUser.Province = dto.Province;
                    weiXinUser.Qr_scene = dto.Qr_scene;
                    weiXinUser.Qr_scene_str = dto.Qr_scene_str;
                    weiXinUser.Remark = dto.Remark;
                    weiXinUser.Sex = dto.Sex;
                    weiXinUser.Subscribe = dto.Subscribe;
                    weiXinUser.Subscribe_scene = dto.Subscribe_scene;
                    weiXinUser.Subscribe_time = dto.Subscribe_time;
                    weiXinUser.Tagid_list = dto.Tagid_list;
                    weiXinUser.LastOperationTime = dto.OperationTime;
                    weiXinUser.LastOperationUserId = dto.OperationUserId;
                    await db.SaveChangesAsync(token);
                    return weiXinUser;
                }
                var entity = db.WeiXinUsers.Add(new WeiXinUser
                {
                    City = dto.City,
                    Country = dto.Country,
                    Groupid = dto.Groupid,
                    Headimgurl = dto.Headimgurl,
                    Language = dto.Language,
                    Nickname = dto.Nickname,
                    OpenId = dto.Openid,
                    Province = dto.Province,
                    Qr_scene = dto.Qr_scene,
                    Qr_scene_str = dto.Qr_scene_str,
                    Remark = dto.Remark,
                    Sex = dto.Sex,
                    Subscribe = dto.Subscribe,
                    Subscribe_scene = dto.Subscribe_scene,
                    Subscribe_time = dto.Subscribe_time,
                    Tagid_list = dto.Tagid_list,
                    Unionid = dto.Unionid,
                    CreateOperationTime = dto.OperationTime,
                    CreateOperationUserId = dto.OperationUserId,
                    LastOperationTime = dto.OperationTime,
                    LastOperationUserId = dto.OperationUserId
                });
                await db.SaveChangesAsync(token);
                return entity;
            }
        }

        public async Task DeleteAsync(WeiXinUserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("微信用户Id不正确！");
                }
                var weiXinUser = await db.WeiXinUsers.Where(x => x.Id == uid && x.IsDeleted == false).FirstOrDefaultAsync(token);
                if (weiXinUser == null)
                {
                    throw new NotImplementedException("微信用户不存在！");
                }

                if (OnDeleteAsync(db, dto, token))
                {
                    throw new NotImplementedException("该微信用户存在下级数据！");
                }

                weiXinUser.LastOperationTime = dto.OperationTime;
                weiXinUser.LastOperationUserId = dto.OperationUserId;
                weiXinUser.DeletedTime = dto.OperationTime;
                weiXinUser.IsDeleted = true;
                await db.SaveChangesAsync(token);
            }
        }

        public async Task<List<WeiXinUser>> GetAllAsync(WeiXinUserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                var list = await db.WeiXinUsers.Where(x => x.IsDeleted == false).ToListAsync(token);

                return list;
            }
        }

        public async Task<WeiXinUser> GetAsync(string unionid, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await db.WeiXinUsers.Where(x => x.Unionid == unionid).FirstOrDefaultAsync(token);
            }
        }

        public Task<List<WeiXinUser>> GetListAsync(WeiXinUserDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(WeiXinUserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (!Guid.TryParse(dto.Id, out var uid))
                {
                    throw new NotImplementedException("微信用户Id不正确！");
                }
                var weiXinUser = await db.WeiXinUsers.Where(x => x.Id == uid).FirstOrDefaultAsync(token);
                if (weiXinUser == null)
                {
                    throw new NotImplementedException("该微信用户不存在！");
                }

                weiXinUser.City = dto.City;
                weiXinUser.Country = dto.Country;
                weiXinUser.Groupid = dto.Groupid;
                weiXinUser.Headimgurl = dto.Headimgurl;
                weiXinUser.Language = dto.Language;
                weiXinUser.Nickname = dto.Nickname;
                weiXinUser.OpenId = dto.Openid;
                weiXinUser.Province = dto.Province;
                weiXinUser.Qr_scene = dto.Qr_scene;
                weiXinUser.Qr_scene_str = dto.Qr_scene_str;
                weiXinUser.Remark = dto.Remark;
                weiXinUser.Sex = dto.Sex;
                weiXinUser.Subscribe = dto.Subscribe;
                weiXinUser.Subscribe_scene = dto.Subscribe_scene;
                weiXinUser.Subscribe_time = dto.Subscribe_time;
                weiXinUser.Tagid_list = dto.Tagid_list;
                weiXinUser.LastOperationTime = dto.OperationTime;
                weiXinUser.LastOperationUserId = dto.OperationUserId;
                OnUpdateAsync(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }

        private void OnUpdateAsync(GuoGuoCommunityContext db, WeiXinUserDto dto, CancellationToken token = default)
        {

        }

        private bool OnDeleteAsync(GuoGuoCommunityContext db, WeiXinUserDto dto, CancellationToken token = default)
        {

            return false;
        }

        public async Task UpdateForUnionIdAsync(WeiXinUserDto dto, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                if (string.IsNullOrWhiteSpace(dto.Unionid))
                {
                    throw new NotImplementedException("微信用户UnionId不正确！");
                }
                var weiXinUser = await db.WeiXinUsers.Where(x => x.Unionid == dto.Unionid).FirstOrDefaultAsync(token);
                if (weiXinUser == null)
                {
                    throw new NotImplementedException("该微信用户不存在！");
                }

                weiXinUser.Subscribe = dto.Subscribe;
                OnUpdateAsync(db, dto, token);
                await db.SaveChangesAsync(token);
            }
        }
    }
}
