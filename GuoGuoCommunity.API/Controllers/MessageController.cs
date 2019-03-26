using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Service;
using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 用来公开给前端用户调用的API
    /// </summary>
    public class MessageController : ApiController
    {
        private readonly ITestRepository _testRepository;

        /// <summary>
        /// 
        /// </summary>
        public MessageController()
        {
            _testRepository = new TestRepository();
        }

        /// <summary>
        /// 这个是用来发送消息的静态方法
        /// </summary>
        /// <param name="message"></param>
        public  async Task SendAsync(string message)
        {
          // await _testRepository.Add(a());
            EventLog.WriteEntry("EventSystem", string.Format("这是由Hangfire后台任务发送的消息:{0},时间为:{1}", message, DateTime.Now));
        }

        public async Task DingShiRenWu()
        {
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"),TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public IHttpActionResult Post(string content)
        {
            //这里可以做一些业务判断或操作

            //然后需要推送的时候，调用下面的方法即可
            BackgroundJob.Enqueue(() => SendAsync(content));

            //最后返回（这里是立即返回，不会阻塞）
            return Ok();
        }

        public static string a()
        {
            string url = "http://dm-51.data.aliyun.com/rest/160601/ocr/ocr_idcard.json";
            string appcode = "9b5eda2cae234efdb318b4344af42782";
            string img_file = "G:\\Download\\20190319215127.jpg";

            //如果输入带有inputs, 设置为True，否则设为False
            bool is_old_format = false;

            //如果没有configure字段，config设为''
            //String config = '';
            string config = "{\\\"side\\\":\\\"face\\\"}";

            string method = "POST";

            string querys = "";

            FileStream fs = new FileStream(img_file, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] contentBytes = br.ReadBytes(Convert.ToInt32(fs.Length));
            string base64 = Convert.ToBase64String(contentBytes);
            string bodys;
            if (is_old_format)
            {
                bodys = "{\"inputs\" :" +
                                    "[{\"image\" :" +
                                        "{\"dataType\" : 50," +
                                         "\"dataValue\" :\"" + base64 + "\"" +
                                         "}";
                if (config.Length > 0)
                {
                    bodys += ",\"configure\" :" +
                                    "{\"dataType\" : 50," +
                                     "\"dataValue\" : \"" + config + "\"}" +
                                     "}";
                }
                bodys += "]}";
            }
            else
            {
                bodys = "{\"image\":\"" + base64 + "\"";
                if (config.Length > 0)
                {
                    bodys += ",\"configure\" :\"" + config + "\"";
                }
                bodys += "}";
            }
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (url.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            //根据API的要求，定义相对应的Content-Type
            httpRequest.ContentType = "application/json; charset=UTF-8";
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("http error code: " + httpResponse.StatusCode);
                Console.WriteLine("error in header: " + httpResponse.GetResponseHeader("X-Ca-Error-Message"));
                Console.WriteLine("error in body: ");
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                var json = reader.ReadToEnd();
                Console.WriteLine(reader.ReadToEnd());
                return json;
            }
            else
            {

                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

                var json = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                //JsonClass jo = (JsonClass)JsonConvert.DeserializeObject(json);
                JsonClass s = JsonConvert.DeserializeObject<JsonClass>(json);
                List<JsonClass> jc = js.Deserialize<List<JsonClass>>(json);
                return json;

            }
            Console.WriteLine("\n");
        }
        public class JsonClass
        {
            public string address { get; set; }
            public string config_str { get; set; }
            //public string face_rect { get; set; }
            //public string face_rect_vertices { get; set; }

            public string name { get; set; }

            public string nationality { get; set; }

            public string num { get; set; }

            public string sex { get; set; }

            public string birth { get; set; }

            public string success { get; set; }
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
