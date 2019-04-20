﻿using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using GuoGuoCommunity.Domain.Service;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 业主认证
    /// </summary>
    public class OwnerCertificationRecordController : ApiController
    {
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IOwnerCertificationAnnexRepository _ownerCertificationAnnexRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly IIDCardPhotoRecordRepository _iDCardPhotoRecordRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="ownerCertificationAnnexRepository"></param>
        /// <param name="ownerRepository"></param>
        /// <param name="industryRepository"></param>
        /// <param name="iDCardPhotoRecordRepository"></param>
        public OwnerCertificationRecordController(
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IOwnerCertificationAnnexRepository ownerCertificationAnnexRepository,
            IOwnerRepository ownerRepository,
            IIndustryRepository industryRepository,
            IIDCardPhotoRecordRepository iDCardPhotoRecordRepository)
        {
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _ownerCertificationAnnexRepository = ownerCertificationAnnexRepository;
            _ownerRepository = ownerRepository;
            _industryRepository = industryRepository;
            _iDCardPhotoRecordRepository = iDCardPhotoRecordRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 阿里云接口地址
        /// </summary>
        public static readonly string ALiYunApiUrl = ConfigurationManager.AppSettings["ALiYunApiUrl"];

        /// <summary>
        /// 阿里云AppCode
        /// </summary>
        public static readonly string ALiYunApiAppCode = ConfigurationManager.AppSettings["ALiYunApiAppCode"];

        /// <summary>
        /// 微信AppID
        /// </summary>
        public static readonly string AppId = ConfigurationManager.AppSettings["GuoGuoCommunity_AppId"];

        /// <summary>
        /// 小程序AppID
        /// </summary>
        public static readonly string GuoGuoCommunity_WxOpenAppId = ConfigurationManager.AppSettings["GuoGuoCommunity_WxOpenAppId"];

        /// <summary>
        /// 微信推送认证结果模板Id
        /// </summary>
        public static readonly string OwnerCertificationRecordTemplateId = ConfigurationManager.AppSettings["OwnerCertificationRecordTemplateId"];

        /// <summary>
        /// 添加业主认证记录信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ownerCertificationRecord/add")]
        public async Task<ApiResult<AddOwnerCertificationRecordOutput>> Add([FromBody]AddOwnerCertificationRecordInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
                }

                if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
                {
                    throw new NotImplementedException("街道办Id信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.CommunityId))
                {
                    throw new NotImplementedException("社区Id信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingId))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingUnitId))
                {
                    throw new NotImplementedException("单元Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.IndustryId))
                {
                    throw new NotImplementedException("业户Id信息为空！");
                }
                if (input.Models.Count < 2)
                {
                    throw new NotImplementedException("提交附件信息不准确！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _ownerCertificationRecordRepository.AddAsync(new OwnerCertificationRecordDto
                {
                    SmallDistrictId = input.SmallDistrictId,
                    IndustryId = input.IndustryId,
                    CommunityId = input.CommunityId,
                    StreetOfficeId = input.StreetOfficeId,
                    BuildingUnitId = input.BuildingUnitId,
                    BuildingId = input.BuildingId,
                    UserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                foreach (var item in input.Models)
                {
                    if (string.IsNullOrWhiteSpace(item.AnnexContent))
                    {
                        continue;
                    }
                    var itemEntity = await _ownerCertificationAnnexRepository.AddAsync(new OwnerCertificationAnnexDto
                    {
                        ApplicationRecordId = entity.Id.ToString(),
                        OwnerCertificationAnnexTypeValue = item.OwnerCertificationAnnexTypeValue,
                        AnnexContent = item.AnnexContent,
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    });
                    
                    if (itemEntity.OwnerCertificationAnnexTypeValue == OwnerCertificationAnnexType.IDCardFront.Value)
                    {
                        // BackgroundJob.Enqueue(() => Send(itemEntity));
                        await Send(itemEntity);
                    }
                }

                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success, new AddOwnerCertificationRecordOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new AddOwnerCertificationRecordOutput { }, e.Message);
            }
        }

        /*
         * 添加后台任务
         * 1.调用阿里云接口
         * 2.存储阿里云接口返回数据
         * 3.调用对比认证申请记录业户信息的业主记录
         * 4.完成认证结果
         */

        /// <summary>
        /// 获取用户认证列表
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ownerCertificationRecord/getList")]
        public async Task<ApiResult<GetListOwnerCertificationRecordOutput>> GetList(CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
                }
                var data = await _ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto
                {
                    UserId = user.Id.ToString()
                }, cancelToken);

                List<GetOwnerCertificationRecordOutput> list = new List<GetOwnerCertificationRecordOutput>();

                foreach (var item in data)
                {
                    var owner = await _ownerRepository.GetAsync(item.OwnerId, cancelToken);
                    var industry = await _industryRepository.GetAsync(item.IndustryId, cancelToken);
                    list.Add(new GetOwnerCertificationRecordOutput
                    {
                        BuildingId = item.BuildingId,
                        BuildingName = item.BuildingName,
                        BuildingUnitId = item.BuildingUnitId,
                        BuildingUnitName = item.BuildingUnitName,
                        CertificationResult = item.CertificationResult,
                        CertificationStatusName = item.CertificationStatusName,
                        CertificationStatusValue = item.CertificationStatusValue,
                        //CertificationTime = item.CertificationTime,
                        CommunityId = item.CommunityId,
                        CommunityName = item.CommunityName,
                        Id = item.Id.ToString(),
                        IndustryId = item.IndustryId,
                        IndustryName = item.IndustryName,
                        OwnerId = item.OwnerId,
                        OwnerName = item.OwnerName,
                        SmallDistrictId = item.SmallDistrictId,
                        SmallDistrictName = item.SmallDistrictName,
                        StreetOfficeId = item.StreetOfficeId,
                        StreetOfficeName = item.StreetOfficeName,
                        UserId = item.UserId,
                        Name = owner?.Name,
                        Birthday = owner?.Birthday,
                        Gender = owner?.Gender,
                        IDCard = owner?.IDCard,
                        PhoneNumber = owner?.PhoneNumber,
                        NumberOfLayers = industry?.NumberOfLayers,
                        Acreage = industry?.Acreage,
                        Oriented = industry?.Oriented
                    });
                }
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success, new GetListOwnerCertificationRecordOutput
                {
                    List = list
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new GetListOwnerCertificationRecordOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 后台执行校验 上传图片认证信息
        /// </summary>
        /// <param name="annex"></param>
        public static async Task<ApiResult> Send(OwnerCertificationAnnex annex)
        {
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
            var ownerCertificationRecordEntity = await ownerCertificationRecordRepository.GetAsync(annex.ApplicationRecordId);
            try
            {
                /*
                 * 查询认证记录
                 */
                OwnerCertificationRecordDto dto = new OwnerCertificationRecordDto
                {
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                    Id = ownerCertificationRecordEntity.Id.ToString()
                };
                IOwnerRepository ownerRepository = new OwnerRepository();

                try
                {
                    /*
                     * 调用阿里云
                     * 根据返回结果查询是否符合认证数据
                     */
                    //EventLog.WriteEntry("EventSystem", string.Format("这里要处理一个图像识别任务:{0},时间为:{1}", message, DateTime.Now));
                    var entity = await PostALiYun(annex);
                    JsonModel json = new JsonModel();
                    try
                    {
                        json = JsonConvert.DeserializeObject<JsonModel>(entity.Message);
                        if (string.IsNullOrEmpty(json.Num))
                        {
                            //更新认证申请记录  有效状态
                            //await ownerCertificationRecordRepository.UpdateInvalidAsync(new OwnerCertificationRecordDto
                            //{
                            //    Id = ownerCertificationRecordEntity.Id.ToString(),
                            //    OperationTime = DateTimeOffset.Now,
                            //    OperationUserId = "system",
                            //});
                            //await ownerCertificationRecordRepository.DeleteAsync(new OwnerCertificationRecordDto
                            //{
                            //    Id = ownerCertificationRecordEntity.Id.ToString(),
                            //    OperationTime = DateTimeOffset.Now,
                            //    OperationUserId = "system",
                            //});
                            throw new NotImplementedException("未识别到身份证信息，请提交正规清晰的身份证照片!");
                            //return new ApiResult { code = APIResultCode.Success_NoB, message = "未识别到身份证信息请提交正规清晰的身份证照片!" };
                        }
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException("未识别到身份证信息，请提交正规清晰的身份证照片!");
                    }


                    var owner = (await ownerRepository.GetListForLegalizeAsync(new OwnerDto { IndustryId = ownerCertificationRecordEntity.IndustryId })).Where(x => x.IDCard == json.Num).FirstOrDefault();

                    if (owner != null)
                    {
                        dto.CertificationStatusValue = OwnerCertification.Success.Value;
                        dto.CertificationStatusName = OwnerCertification.Success.Name;
                        dto.OwnerId = owner.Id.ToString();
                        dto.OwnerName = owner.Name.ToString();
                        dto.CertificationResult = "认证通过";
                    }
                    else
                    {
                        dto.CertificationStatusValue = OwnerCertification.Failure.Value;
                        dto.CertificationStatusName = OwnerCertification.Failure.Name;
                        dto.CertificationResult = "未查询到相关业主信息";
                    }

                }
                catch (Exception e)
                {
                    dto.CertificationStatusValue = OwnerCertification.Failure.Value;
                    dto.CertificationStatusName = OwnerCertification.Failure.Name;
                    dto.CertificationResult = e.Message;
                    //更新认证申请记录  有效状态
                    //await ownerCertificationRecordRepository.UpdateInvalidAsync(new OwnerCertificationRecordDto
                    //{
                    //    Id = ownerCertificationRecordEntity.Id.ToString(),
                    //    OperationTime = DateTimeOffset.Now,
                    //    OperationUserId = "system",
                    //});
                    //await ownerCertificationRecordRepository.DeleteAsync(new OwnerCertificationRecordDto
                    //{
                    //    Id = ownerCertificationRecordEntity.Id.ToString(),
                    //    OperationTime = DateTimeOffset.Now,
                    //    OperationUserId = "system",
                    //});
                    throw new NotImplementedException(e.Message);
                    //return new ApiResult { code = APIResultCode.Success_NoB, message = e.Message };
                    // throw new NotImplementedException("身份证信息读取错误！");
                }

                var recordEntity = await ownerCertificationRecordRepository.UpdateAsync(dto);

                if (string.IsNullOrWhiteSpace(dto.OwnerId))
                {
                    ////更新认证申请记录  有效状态
                    //await ownerCertificationRecordRepository.UpdateInvalidAsync(new OwnerCertificationRecordDto
                    //{
                    //    Id = ownerCertificationRecordEntity.Id.ToString(),
                    //    OperationTime = DateTimeOffset.Now,
                    //    OperationUserId = "system",
                    //});
                    //await ownerCertificationRecordRepository.DeleteAsync(new OwnerCertificationRecordDto
                    //{
                    //    Id = ownerCertificationRecordEntity.Id.ToString(),
                    //    OperationTime = DateTimeOffset.Now,
                    //    OperationUserId = "system",
                    //});
                    throw new NotImplementedException("未查询到相关业主信息");
                    //return new ApiResult { code = APIResultCode.Success_NoB, message = "未查询到相关业主信息!" };
                }
                //回写用户认证记录到业主信息
                await ownerRepository.UpdateForLegalizeAsync(new OwnerDto
                {
                    OwnerCertificationRecordId = ownerCertificationRecordEntity.Id.ToString(),
                    Id = recordEntity.OwnerId.ToString()
                });

                //IUserRepository userRepository = new UserRepository();
                //var userEntity = await userRepository.GetForIdAsync(recordEntity.UserId);
                ////微信推送
                //IWeiXinUserRepository weiXinUserRepository = new WeiXinUserRepository();
                //var weiXinUser = await weiXinUserRepository.GetAsync(userEntity.UnionId);
                //OwnerCertificationRecordPushRemind(new OwnerCertificationRecordPushModel
                //{
                //    OpenId = weiXinUser.OpenId,
                //    Status = dto.CertificationStatusName,
                //    Message = dto.CertificationResult
                //});
                return new ApiResult { code = APIResultCode.Success };
            }
            catch (Exception e)
            {
                //更新认证申请记录  有效状态
                await ownerCertificationRecordRepository.UpdateInvalidAsync(new OwnerCertificationRecordDto
                {
                    Id = ownerCertificationRecordEntity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                });
                await ownerCertificationRecordRepository.DeleteAsync(new OwnerCertificationRecordDto
                {
                    Id = ownerCertificationRecordEntity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                });
                throw new NotImplementedException(e.Message);
                //return new ApiResult { code = APIResultCode.Success_NoB, message = e.Message };
                //throw new NotImplementedException("检测到未知错误，请联系管理员！");
            }

        }

        /// <summary>
        /// 调用阿里云接口
        /// </summary>
        /// <param name="annex"></param>
        /// <returns></returns>
        public async static Task<IDCardPhotoRecord> PostALiYun(OwnerCertificationAnnex annex)
        {
            string aLiYunApiUrl = ALiYunApiUrl;
            string appcode = ALiYunApiAppCode;
            //查询附件url
            IOwnerCertificationAnnexRepository ownerCertificationAnnexRepository = new OwnerCertificationAnnexRepository();
            var url = ownerCertificationAnnexRepository.GetPath(annex.Id.ToString());
            string img_file = HttpRuntime.AppDomainAppPath.ToString() + url;


            //如果输入带有inputs, 设置为True，否则设为False
            bool is_old_format = false;

            //如果没有configure字段，config设为''
            //String config = '';
            string config = "{\\\"side\\\":\\\"face\\\"}";

            string method = "POST";

            string querys = "";

            using (FileStream fs = new FileStream(img_file, FileMode.Open))
            {
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
                    aLiYunApiUrl = aLiYunApiUrl + "?" + querys;
                }

                if (aLiYunApiUrl.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(aLiYunApiUrl));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(aLiYunApiUrl);
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
                string json;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("http error code: " + httpResponse.StatusCode);
                    Console.WriteLine("error in header: " + httpResponse.GetResponseHeader("X-Ca-Error-Message"));
                    Console.WriteLine("error in body: ");
                    Stream st = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                    json = reader.ReadToEnd();
                    Console.WriteLine(reader.ReadToEnd());
                }
                else
                {
                    Stream st = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                    json = reader.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //JsonClass jo = (JsonClass)JsonConvert.DeserializeObject(json);
                    JsonModel s = JsonConvert.DeserializeObject<JsonModel>(json);
                    //List<JsonClass> jc = js.Deserialize<List<JsonClass>>(json);
                }
                IIDCardPhotoRecordRepository iDCardPhotoRecordRepository = new IDCardPhotoRecordRepository();
                var entity = await iDCardPhotoRecordRepository.AddAsync(new IDCardPhotoRecordDto
                {
                    ApplicationRecordId = annex.ApplicationRecordId,
                    OwnerCertificationAnnexId = annex.Id.ToString(),
                    Message = json,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                    PhotoBase64 = base64,
                });
                return entity;
            }

        }

        /// <summary>
        /// 认证结果通知
        /// </summary>
        public static void OwnerCertificationRecordPushRemind(OwnerCertificationRecordPushModel model)
        {
            try
            {
                var templateData = new
                {
                    first = new TemplateDataItem("用户认证通知"),
                    keyword1 = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                    keyword2 = new TemplateDataItem(model.Status),
                    keyword3 = new TemplateDataItem(model.Message),
                    remark = new TemplateDataItem("详情", "#FF0000")
                };

                var miniProgram = new TempleteModel_MiniProgram()
                {
                    appid = GuoGuoCommunity_WxOpenAppId,
                    pagepath = "pages/my/my"
                };

                TemplateApi.SendTemplateMessage(AppId, model.OpenId, OwnerCertificationRecordTemplateId, null, templateData, miniProgram);
            }
            catch (Exception e)
            {

            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete]
        public string GetPath(string id)
        {
            DirectoryInfo rootDir = Directory.GetParent(Environment.CurrentDirectory);
            string root = rootDir.Parent.Parent.FullName;
            string a = HttpRuntime.AppDomainAppPath.ToString();
            return HttpRuntime.AppDomainAppPath.ToString();// _ownerCertificationAnnexRepository.GetPath(id);
        }

        /// <summary>
        /// 根据小区id获取用户id
        /// </summary>
        /// <param name="SmallDistrictId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("ownerCertificationRecord/getAllForSmallDistrictId2")]
        public async Task<ApiResult<GetListOwnerCertificationRecordOutput>> GetAllForSmallDistrictId([FromUri]string SmallDistrictId, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
                }
                var data = await _ownerCertificationRecordRepository.GetAllForSmallDistrictIdAsync(new OwnerCertificationRecordDto
                {
                    UserId = user.Id.ToString(),
                    SmallDistrictId = SmallDistrictId
                }, cancelToken);
                List<GetOwnerCertificationRecordOutput> list = new List<GetOwnerCertificationRecordOutput>();

                foreach (var item in data)
                {
                    var owner = await _ownerRepository.GetAsync(item.OwnerId, cancelToken);
                    var industry = await _industryRepository.GetAsync(item.IndustryId, cancelToken);
                    list.Add(new GetOwnerCertificationRecordOutput
                    {
                        BuildingId = item.BuildingId,
                        BuildingName = item.BuildingName,
                        BuildingUnitId = item.BuildingUnitId,
                        BuildingUnitName = item.BuildingUnitName,
                        CertificationResult = item.CertificationResult,
                        CertificationStatusName = item.CertificationStatusName,
                        CertificationStatusValue = item.CertificationStatusValue,
                        CertificationTime = item.CreateOperationTime.ToString(),
                        CommunityId = item.CommunityId,
                        CommunityName = item.CommunityName,
                        Id = item.Id.ToString(),
                        IndustryId = item.IndustryId,
                        IndustryName = item.IndustryName,
                        OwnerId = item.OwnerId,
                        OwnerName = item.OwnerName,
                        SmallDistrictId = item.SmallDistrictId,
                        SmallDistrictName = item.SmallDistrictName,
                        StreetOfficeId = item.StreetOfficeId,
                        StreetOfficeName = item.StreetOfficeName,
                        UserId = item.UserId,
                        Name = owner?.Name,
                        Birthday = owner?.Birthday,
                        Gender = owner?.Gender,
                        IDCard = owner?.IDCard,
                        PhoneNumber = owner?.PhoneNumber,
                        NumberOfLayers = industry?.NumberOfLayers,
                        Acreage = industry?.Acreage
                    });
                }
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success, new GetListOwnerCertificationRecordOutput
                {
                    List = list
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new GetListOwnerCertificationRecordOutput { }, e.Message);
            }
        }
    }
}
