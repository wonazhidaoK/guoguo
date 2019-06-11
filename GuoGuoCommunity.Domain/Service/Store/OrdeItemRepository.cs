using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class OrdeItemRepository : IOrdeItemRepository
    {
        public Task<OrderItem> AddAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetAllAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetAllIncludeAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetIncludeAsync(string id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetListAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetListIncludeAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderItem>> GetListIncludeForOrderIdAsync(string id, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await (from x in db.OrdeItems where x.OrderId.ToString() == id select x).ToListAsync(token);
            }
        }

        public async Task<List<OrderItem>> GetListIncludeForOrderIdsAsync(List<string> ids, CancellationToken token = default)
        {
            using (var db = new GuoGuoCommunityContext())
            {
                return await (from x in db.OrdeItems where ids.Contains(x.OrderId.ToString()) select x).ToListAsync(token);
            }
        }

        public Task UpdateAsync(OrdeItemDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
