using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services.Shared
{
    public class RequestChangeSelectQuery
    {
        public string Sender { get; set; }
        public string Filter { get; set; }
    }

    public class RequestChangeSelectDTO
    {
        public IEnumerable<RequestChange> RequestChanges { get; set; }
        public int Count { get; set; }

        public class RequestChange
        {
            public Guid Id { get; set; }
            public Guid ShiftId { get; set; }
            public string Motivation { get; set; }
            public RequestState State { get; set; }
            public DateTime SentDate { get; set; }
        }
    }

    public class RequestChangeDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class RequestChangeDetailDTO
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public string Motivation { get; set; }
        public RequestState State { get; set; }
        public DateTime SentDate { get; set; }
        public string Sender { get; set; }
    }

    public partial class SharedService
    {
        public async Task<RequestChangeSelectDTO> Query(RequestChangeSelectQuery qry)
        {
            var queryable = _dbContext.RequestsChanges.AsQueryable();

            if (!string.IsNullOrWhiteSpace(qry.Sender))
            {
                queryable = queryable.Where(x => x.Sender == qry.Sender);
            }

            if (!string.IsNullOrWhiteSpace(qry.Filter))
            {
                queryable = queryable.Where(x => x.Motivation.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new RequestChangeSelectDTO
            {
                RequestChanges = await queryable
                    .Select(x => new RequestChangeSelectDTO.RequestChange
                    {
                        Id = x.Id,
                        ShiftId = x.ShiftId,
                        Motivation = x.Motivation,
                        State = x.State,
                        SentDate = x.SentDate
                    })
                    .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        public async Task<RequestChangeDetailDTO> Query(RequestChangeDetailQuery qry)
        {
            return await _dbContext.RequestsChanges
                .Where(x => x.Id == qry.Id)
                .Select(x => new RequestChangeDetailDTO
                {
                    Id = x.Id,
                    ShiftId = x.ShiftId,
                    Motivation = x.Motivation,
                    State = x.State,
                    SentDate = x.SentDate,
                    Sender = x.Sender
                })
                .FirstOrDefaultAsync();
        }
    }
}
