﻿using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IOwnerRepository : IIncludeRepository<Owner, OwnerDto>
    {
        Task UpdateForLegalizeAsync(OwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据业主id集合查询业主集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Owner>> GetForIdsAsync(List<string> ids, CancellationToken token = default);

        /// <summary>
        /// 根据业主id集合查询业主集合
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Owner>> GetForIdsIncludeAsync(List<string> ids, CancellationToken token = default);

        /// <summary>
        /// 获取业户下未认证业主信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Owner>> GetListForLegalizeIncludeAsync(OwnerDto dto, CancellationToken token = default);

        /// <summary>
        /// 根据业主认证申请id查询业主
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Owner> GetForOwnerCertificationRecordIdAsync(OwnerDto dto, CancellationToken token = default);
    }
}
