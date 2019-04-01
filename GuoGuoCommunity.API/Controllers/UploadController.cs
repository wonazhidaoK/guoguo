using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UploadController : ApiController
    {
        private readonly IUploadRepository _uploadRepository;
        private TokenManager _tokenManager;
        private static readonly string host = ConfigurationManager.AppSettings["Host"];
        private static readonly string agreement = ConfigurationManager.AppSettings["Agreement"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadRepository"></param>
        public UploadController(IUploadRepository uploadRepository)
        {
            _uploadRepository = uploadRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Obsolete]
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
        /// 业主认证上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadOwnerCertification")]
        public async Task<ApiResult<UploadOutput>> UploadOwnerCertification(CancellationToken cancelToken)
        {
            try
            {
                #region Token
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }
                #endregion

                //if (string.IsNullOrWhiteSpace(type))
                //{
                //    throw new NotImplementedException("上传附件类型为空！");
                //}

                string typeName = "OwnerCertification";
                //switch (type)
                //{
                //    case "Owner":
                //        typeName = "OwnerCertification";
                //        break;
                //    case "VipOwner":
                //        typeName = "VipOwnerCertification";
                //        break;
                //    case "Announcement":
                //        typeName = "Announcement";
                //        break;
                //    default:
                //        break;
                //}

                // Check whether the POST operation is MultiPart?
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                // Prepare CustomMultipartFormDataStreamProvider in which our multipart form
                // data will be loaded.
                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.
                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(await AddUpload(typeName, file.Headers.ContentDisposition.FileName.Trim('"'), user.Id.ToString(), cancelToken));
                }

                // Send OK Response along with saved file names to the client.
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 高级认证上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadVipOwnerCertificationRecord")]
        public async Task<ApiResult<UploadOutput>> UploadVipOwnerCertificationRecord(CancellationToken cancelToken)
        {
            try
            {
                #region Token
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }
                #endregion

                string typeName = "VipOwnerCertification";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(await AddUpload(typeName, file.Headers.ContentDisposition.FileName.Trim('"'), user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 公告上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadAnnouncement")]
        public async Task<ApiResult<UploadOutput>> UploadAnnouncement(CancellationToken cancelToken)
        {
            try
            {
                #region Token
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }
                #endregion

                string typeName = "Announcement";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(await AddUpload(typeName, file.Headers.ContentDisposition.FileName.Trim('"'), user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 投票上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadVote")]
        public async Task<ApiResult<UploadOutput>> UploadVote(CancellationToken cancelToken)
        {
            try
            {
                #region Token
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }
                #endregion

                string typeName = "Vote";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(await AddUpload(typeName, file.Headers.ContentDisposition.FileName.Trim('"'), user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="cancelToken"></param>
        private async Task<UploadOutput> AddUpload(string directory, string file, string userId, CancellationToken cancelToken = default)
        {
            var upload = await _uploadRepository.AddAsync(
                  new UploadDto
                  {
                      Agreement = agreement + "://",
                      Host = host + "/",
                      Domain = "/Upload",
                      Directory = "/" + directory,
                      File = "/" + file,
                      OperationTime = DateTimeOffset.Now,
                      OperationUserId = userId
                  }, cancelToken);
            if (upload != null)
            {
                return new UploadOutput { Id = upload.Id.ToString(), Url = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File };
            }
            return new UploadOutput { };
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
