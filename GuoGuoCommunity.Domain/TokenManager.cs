using GuoGuoCommunity.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Text;
namespace GuoGuoCommunity.Domain
{
    public class TokenManager
    {
        //密钥从数据库获取
        public string key = "AAAAAAAAAA-BBBBBBBBBB-CCCCCCCCCC-DDDDDDDDDD-EEEEEEEEEE-FFFFFFFFFF-GGGGGGGGGG";



        //产生 Token
        public Token Create(User user)
        {
            var exp = 36000;   //过期时间(秒)

            //稍微修改 Payload 将使用着咨询和过期时间翻开
            var payload = new Payload
            {
                info = user,
                //Unix 时间戳
                exp = Convert.ToInt32((DateTime.Now.AddSeconds(exp) - new DateTime(1970, 1, 1)).TotalSeconds)
            };

            var json = JsonConvert.SerializeObject(payload);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            var iv = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);

            //使用 AES 加密 Payload
            var encrypt = TokenCrypto.AESEncrypt(base64, key.Substring(0, 16), iv);

            //取得签章
            var signature = TokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, key.Substring(0, 64));

            return new Token
            {
                //Token 為 iv + encrypt + signature，並用 . 串聯
                access_token = iv + "." + encrypt + "." + signature,
                //Refresh Token 使用 Guid 產生
                refresh_token = Guid.NewGuid().ToString().Replace("-", ""),
                expires_in = exp,
            };
        }

        //取得使用者咨询
        public User GetUser(string token)
        {
            var split = token.Split('.');
            var iv = split[0];
            var encrypt = split[1];
            var signature = split[2];

            //检查签章是否正确
            if (signature != TokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, key.Substring(0, 64)))
            {
                return null;
            }

            //使用 AES 解密 Payload
            var base64 = TokenCrypto
                .AESDecrypt(encrypt, key.Substring(0, 16), iv);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            var payload = JsonConvert.DeserializeObject<Payload>(json);

            //检查是否过期
            if (payload.exp < Convert.ToInt32(
                (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds))
            {
                return null;
            }

            return payload.info;
        }
    }
}
