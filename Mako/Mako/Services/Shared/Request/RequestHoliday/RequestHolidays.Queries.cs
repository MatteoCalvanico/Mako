using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services.Shared
{
    public class RequestHolidaySelectQuery
    {
        public string Sender { get; set; }
        public DateTime? StartDateFilter { get; set; }
        public DateTime? EndDateFilter { get; set; }
    }

    public class RequestHolidaySelectDTO
    {
        public IEnumerable<RequestHoliday> RequestHolidays { get; set; }
        public int Count { get; set; }

        public class RequestHoliday
        {
            public Guid Id { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Motivation { get; set; }
            public RequestState State { get; set; }
            public DateTime SentDate { get; set; }
        }
    }

    public class RequestHolidayDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class RequestHolidayDetailDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Motivation { get; set; }
        public RequestState State { get; set; }
        public DateTime SentDate { get; set; }
        public string Sender { get; set; }
    }

    public partial class SharedService
    {
        public async Task<RequestHolidaySelectDTO> Query(RequestHolidaySelectQuery qry)
        {
            var queryable = _dbContext.RequestsHolidays.AsQueryable();

            if (!string.IsNullOrWhiteSpace(qry.Sender))
            {
                queryable = queryable.Where(x => x.Sender == qry.Sender);
            }

            if (qry.StartDateFilter.HasValue)
            {
                queryable = queryable.Where(x => x.StartDate >= qry.StartDateFilter.Value);
            }

            if (qry.EndDateFilter.HasValue)
            {
                queryable = queryable.Where(x => x.EndDate <= qry.EndDateFilter.Value);
            }

            return new RequestHolidaySelectDTO
            {
                RequestHolidays = await queryable
                    .Select(x => new RequestHolidaySelectDTO.RequestHoliday
                    {
                        Id = x.Id,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Motivation = x.Motivation,
                        State = x.State,
                        SentDate = x.SentDate
                    })
                    .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        public async Task<RequestHolidayDetailDTO> Query(RequestHolidayDetailQuery qry)
        {
            return await _dbContext.RequestsHolidays
                .Where(x => x.Id == qry.Id)
                .Select(x => new RequestHolidayDetailDTO
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Motivation = x.Motivation,
                    State = x.State,
                    SentDate = x.SentDate,
                    Sender = x.Sender
                })
                .FirstOrDefaultAsync();
        }
    }
}
