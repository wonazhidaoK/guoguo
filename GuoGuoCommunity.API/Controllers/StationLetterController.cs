using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 站内信相关
    /// </summary>
    public class StationLetterController : BaseController
    {
        private readonly IStationLetterRepository _stationLetterRepository;
        private readonly IStationLetterAnnexRepository _stationLetterAnnexRepository;
        private readonly IStationLetterBrowseRecordRepository _stationLetterBrowseRecordRepository;
        private readonly IUserRepository _userRepository;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationLetterRepository"></param>
        /// <param name="stationLetterAnnexRepository"></param>
        /// <param name="stationLetterBrowseRecordRepository"></param>
        /// <param name="userRepository"></param>
        public StationLetterController(
            IStationLetterRepository stationLetterRepository,
            IStationLetterAnnexRepository stationLetterAnnexRepository,
            IStationLetterBrowseRecordRepository stationLetterBrowseRecordRepository,
            IUserRepository userRepository)
        {
            _stationLetterRepository = stationLetterRepository;
            _stationLetterAnnexRepository = stationLetterAnnexRepository;
            _stationLetterBrowseRecordRepository = stationLetterBrowseRecordRepository;
            _userRepository = userRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加站内信(街道办后台)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("stationLetter/add")]
        public async Task<ApiResult<AddStationLetterOutput>> Add([FromBody]AddStationLetterInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddStationLetterOutput>(APIResultCode.Unknown, new AddStationLetterOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Content))
            {
                throw new NotImplementedException("站内信内容信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.SmallDistrictArray))
            {
                throw new NotImplementedException("站内信小区范围信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new NotImplementedException("站内信标题信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddStationLetterOutput>(APIResultCode.Unknown, new AddStationLetterOutput { }, APIResultMessage.TokenError);
            }
            var entity = await _stationLetterRepository.AddAsync(new StationLetterDto
            {
                Title = input.Title,
                Summary = input.Summary,
                SmallDistrictArray = input.SmallDistrictArray,
                Content = input.Content,
                StreetOfficeId = user.StreetOfficeId,
                StreetOfficeName = user.StreetOfficeName,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            if (!string.IsNullOrWhiteSpace(input.AnnexId))
            {
                await _stationLetterAnnexRepository.AddAsync(new StationLetterAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    StationLetterId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
            }

            return new ApiResult<AddStationLetterOutput>(APIResultCode.Success, new AddStationLetterOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 街道办获取站内信列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("stationLetter/getAllStreetOfficeStationLetter")]
        public async Task<ApiResult<GetAllStreetOfficeStationLetterOutput>> GetAllStreetOfficeStationLetter([FromUri]GetAllStreetOfficeStationLetterInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Unknown, new GetAllStreetOfficeStationLetterOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Unknown, new GetAllStreetOfficeStationLetterOutput { }, APIResultMessage.TokenError);
            }

            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }
            var startTime = DateTimeOffset.Parse("1997-01-01");

            var endTime = DateTimeOffset.Parse("2997-01-01");

            if (DateTimeOffset.TryParse(input.StartTime, out DateTimeOffset startTimeSet))
            {
                startTime = startTimeSet;
            }
            if (DateTimeOffset.TryParse(input.EndTime, out DateTimeOffset endTimeSet))
            {
                endTime = endTimeSet.AddDays(1).AddMinutes(-1);
            }

            int startRow = (input.PageIndex - 1) * input.PageSize;

            var data = await _stationLetterRepository.GetAllAsync(new StationLetterDto
            {
                ReleaseTimeEnd = endTime,
                ReleaseTimeStart = startTime,
                StreetOfficeId = user.StreetOfficeId,
                SmallDistrictArray = input.SmallDistrictId
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);
            List<GetStationLetterOutput> listOutput = new List<GetStationLetterOutput>();
            var userList = await _userRepository.GetByIdsAsync(list.Select(x => x.CreateOperationUserId).ToList());
            foreach (var item in list)
            {
                var userEntity = userList.Where(x => x.Id.ToString() == item.CreateOperationUserId).FirstOrDefault();

                listOutput.Add(new GetStationLetterOutput
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    StreetOfficeId = item.StreetOfficeId,
                    Summary = item.Summary,
                    StreetOfficeName = item.StreetOfficeName,
                    ReleaseTime = item.CreateOperationTime.Value,
                    CreateUserName = userEntity?.Name
                });
            }

            return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Success, new GetAllStreetOfficeStationLetterOutput
            {
                List = listOutput,
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 街道办获取站内信详情
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("stationLetter/get")]
        public async Task<ApiResult<GetStationLetterOutput>> Get([FromUri]GetPropertyStationLetterInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetStationLetterOutput>(APIResultCode.Unknown, new GetStationLetterOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);

            if (user == null)
            {
                return new ApiResult<GetStationLetterOutput>(APIResultCode.Unknown, new GetStationLetterOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _stationLetterRepository.GetAsync(input.Id, cancelToken);

            var userEntity = await _userRepository.GetForIdAsync(entity.CreateOperationUserId);

            return new ApiResult<GetStationLetterOutput>(APIResultCode.Success, new GetStationLetterOutput
            {
                Id = entity.Id.ToString(),
                Title = entity.Title,
                Content = entity.Content,
                StreetOfficeId = entity.StreetOfficeId,
                Summary = entity.Summary,
                StreetOfficeName = entity.StreetOfficeName,
                Url = _stationLetterAnnexRepository.GetUrl(entity.Id.ToString()),
                CreateUserName = userEntity?.Name
            });

        }

        /// <summary>
        /// 物业获取站内信列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("stationLetter/getAllPropertyStationLetter")]
        public async Task<ApiResult<GetAllPropertyStationLetterOutput>> GetAllPropertyStationLetter([FromUri]GetAllPropertyStationLetterInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllPropertyStationLetterOutput>(APIResultCode.Unknown, new GetAllPropertyStationLetterOutput { }, APIResultMessage.TokenNull);
            }

            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }

            var startTime = DateTimeOffset.Parse("1997-01-01");

            var endTime = DateTimeOffset.Parse("2997-01-01");

            if (DateTimeOffset.TryParse(input.StartTime, out DateTimeOffset startTimeSet))
            {
                startTime = startTimeSet;
            }
            if (DateTimeOffset.TryParse(input.EndTime, out DateTimeOffset endTimeSet))
            {
                endTime = endTimeSet.AddDays(1).AddMinutes(-1);
            }

            int startRow = (input.PageIndex - 1) * input.PageSize;

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllPropertyStationLetterOutput>(APIResultCode.Unknown, new GetAllPropertyStationLetterOutput { }, APIResultMessage.TokenError);
            }

            var data = await _stationLetterRepository.GetAllForPropertyAsync(new StationLetterDto
            {
                ReleaseTimeEnd = endTime,
                ReleaseTimeStart = startTime,
                SmallDistrictArray = user.SmallDistrictId,
                OperationUserId = user.Id.ToString(),
                ReadStatus = input.ReadStatus
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);
            var userList = await _userRepository.GetByIdsAsync(list.Select(x => x.CreateOperationUserId).ToList());
            var stationLetterBrowseRecordList = await _stationLetterBrowseRecordRepository.GetForStationLetterIdsAsync(list.Select(x => x.Id.ToString()).ToList());
            List<GetPropertyStationLetterOutput> listOutput = new List<GetPropertyStationLetterOutput>();
            foreach (var item in list)
            {

                var userEntity = userList.Where(x => x.Id.ToString() == item.CreateOperationUserId).FirstOrDefault();
                var userBrowseRecordList = stationLetterBrowseRecordList.Where(x => x.StationLetterId == item.Id.ToString() && x.CreateOperationUserId == user.Id.ToString());
                listOutput.Add(new GetPropertyStationLetterOutput
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    StreetOfficeId = item.StreetOfficeId,
                    Summary = item.Summary,
                    StreetOfficeName = item.StreetOfficeName,
                    ReleaseTime = item.CreateOperationTime.Value,
                    CreateUserName = userEntity?.Name,
                    IsRead = userBrowseRecordList.Any()
                });
            }
            return new ApiResult<GetAllPropertyStationLetterOutput>(APIResultCode.Success, new GetAllPropertyStationLetterOutput
            {
                List = listOutput,
                TotalCount = data.Count()
            });
        }

        /// <summary>
        /// 物业获取站内信详情
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("stationLetter/getPropertyStationLetter")]
        public async Task<ApiResult<GetStationLetterOutput>> GetPropertyStationLetter([FromUri]GetPropertyStationLetterInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetStationLetterOutput>(APIResultCode.Unknown, new GetStationLetterOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetStationLetterOutput>(APIResultCode.Unknown, new GetStationLetterOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _stationLetterRepository.GetAsync(input.Id, cancelToken);
            await _stationLetterBrowseRecordRepository.AddAsync(new StationLetterBrowseRecordDto
            {
                StationLetterId = entity.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            });

            var userEntity = await _userRepository.GetForIdAsync(entity.CreateOperationUserId);
            return new ApiResult<GetStationLetterOutput>(APIResultCode.Success, new GetStationLetterOutput
            {
                Id = entity.Id.ToString(),
                Title = entity.Title,
                Content = entity.Content,
                StreetOfficeId = entity.StreetOfficeId,
                Summary = entity.Summary,
                StreetOfficeName = entity.StreetOfficeName,
                Url = _stationLetterAnnexRepository.GetUrl(entity.Id.ToString()),
                CreateUserName = userEntity?.Name
            });
        }
    }
}
