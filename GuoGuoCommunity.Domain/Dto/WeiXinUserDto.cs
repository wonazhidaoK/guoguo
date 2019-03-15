using System;

namespace GuoGuoCommunity.Domain.Dto
{
    public class WeiXinUserDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 用户关注渠道
        /// </summary>
        public string Subscribe_scene { get; set; }

        /// <summary>
        /// 用户标签
        /// </summary>
        public string Tagid_list { get; set; }

        /// <summary>
        /// 用户分组
        /// </summary>
        public string Groupid { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 微信用户唯一标识
        /// </summary>
        public string Unionid { get; set; }

        /// <summary>
        /// 用户关注时间
        /// </summary>
        public string Subscribe_time { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Headimgurl { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///   用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        ///  用户所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        ///   用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int Subscribe { get; set; }

        /// <summary>
        ///  二维码扫码场景（开发者自定义）
        /// </summary>
        public int Qr_scene { get; set; }

        /// <summary>
        ///  二维码扫码场景描述（开发者自定义）
        /// </summary>
        public string Qr_scene_str { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public string OperationUserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? OperationTime { get; set; }
    }
}
