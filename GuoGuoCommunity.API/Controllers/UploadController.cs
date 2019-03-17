using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using System.Web.Mvc;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UploadController : ApiController
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
       // [HttpGet]
        [Route("upload/image")]
        public string UploadImage()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpPostedFile filess = HttpContext.Current.Request.Files["card"];//接收
            string sFileName = AppDomain.CurrentDomain.BaseDirectory + filess.FileName;

            filess.SaveAs(sFileName);
            //var msg = bll.UploadImage(file.InputStream, userID);
            //var result = new ReturnResult<string>(msg);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("upload/post")]
        public async Task<HttpResponseMessage> Post()
        {
            // Check whether the POST operation is MultiPart?
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Prepare CustomMultipartFormDataStreamProvider in which our multipart form
            // data will be loaded.
            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/App_Data");
            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            List<string> files = new List<string>();

            try
            {
                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(Path.GetFileName(file.LocalFileName));
                }

                // Send OK Response along with saved file names to the client.
                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="headers"></param>
            /// <returns></returns>
            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }
    }



}
