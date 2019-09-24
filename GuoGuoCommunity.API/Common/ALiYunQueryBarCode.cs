using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GuoGuoCommunity.API.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ALiYunQueryBarCode
    {
        private const string host = "https://codequery.market.alicloudapi.com";
        private const string path = "/querybarcode";
        private const string method = "GET";
        private const string appcode = "2cc46df2e6654be1a9880db4801a1c9b";


        /// <summary>
        /// 6902083881405
        /// </summary>
        public static ResultModel Query(string code)
        {
            string querys = "code=" + code;
            string bodys = "";
            string url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
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

            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            var commodityModel = JsonConvert.DeserializeObject<CommodityModel>(reader.ReadToEnd());
            if (commodityModel.Status=="200")
            {
                return commodityModel.Result;
            }
            return new ResultModel { };
            //Trace.WriteLine(reader.ReadToEnd());
            //Console.WriteLine("\n");

        }
        public class CommodityModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Msg { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public ResultModel Result { get; set; }

            /// <summary>
            /// 
            /// </summary>
           
        }
        public class ResultModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string GoodsName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ManuName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ManuAddress { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Spec { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Price { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Img { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string GoodsType { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Ycg { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Trademark { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Temark { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}