using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    public class UploadController : BaseController
    {
        private readonly IUploadRepository _uploadRepository;
        private readonly TokenManager _tokenManager;

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

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "OwnerCertification";
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
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

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
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

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 投诉上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadComplaint")]
        public async Task<ApiResult<UploadOutput>> UploadComplaint(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "Complaint";

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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
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

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 站内信上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadStationLetter")]
        public async Task<ApiResult<UploadOutput>> UploadStationLetter(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "StationLetter";

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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }



        /// <summary>
        /// 上传物业公司Logo图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadPropertyCompany")]
        public async Task<ApiResult<UploadOutput>> UploadPropertyCompany(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "PropertyCompany";
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 商户账户创建上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadShop")]
        public async Task<ApiResult<UploadOutput>> UploadShop(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "Shop";

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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 上传商户商品图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadShopCommodity")]
        public async Task<ApiResult<UploadOutput>> UploadShopCommodity(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "ShopCommodity";
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 上传平台商品图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadPlatformCommodityCertification")]
        public async Task<ApiResult<UploadOutput>> UploadPlatformCommodityCertification(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "PlatformCommodityCertification";
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
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
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
                      Agreement = Agreement + "://",
                      Host = Host + "/",
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
        /// 服务器地址和业务存储地址分开返回
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="cancelToken"></param>
        private async Task<UploadOutput> NewAddUpload(string directory, string file, string userId, CancellationToken cancelToken = default)
        {
            var upload = await _uploadRepository.AddAsync(
                  new UploadDto
                  {
                      Agreement = Agreement + "://",
                      Host = Host + "/",
                      Domain = "/Upload",
                      Directory = "/" + directory,
                      File = "/" + file,
                      OperationTime = DateTimeOffset.Now,
                      OperationUserId = userId
                  }, cancelToken);
            if (upload != null)
            {
                return new UploadOutput { Id = upload.Id.ToString(), Url = upload.Directory + upload.File };
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
            return "";
        }
    }
}
