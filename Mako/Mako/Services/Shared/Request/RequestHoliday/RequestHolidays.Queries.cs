using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services.Shared
{
    public class RequestHolidaySelectQuery
    {
        public Guid ShipCurrentId { get; set; }
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
            public string WorkerCf { get; set; }
            public string WorkerName { get; set; }
            public string WorkerSurname { get; set; }
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
        public string WorkerCf { get; set; }
    }

    public partial class SharedService
    {
        public async Task<RequestHolidaySelectDTO> SelectRequestsHolidayQuery(RequestHolidaySelectQuery qry)
        {
            var holidayQuery = _dbContext.RequestsHolidays.AsQueryable();

            var resultList = await holidayQuery
                .Join(_dbContext.Workers,
                    holiday => holiday.WorkerCf,
                    worker => worker.Cf,
                    (holiday, worker) => new RequestHolidaySelectDTO.RequestHoliday
                    {
                        Id = holiday.Id,
                        StartDate = holiday.StartDate,
                        EndDate = holiday.EndDate,
                        Motivation = holiday.Motivation,
                        State = holiday.State,
                        SentDate = holiday.SentDate,
                        WorkerCf = holiday.WorkerCf,
                        WorkerName = worker.Name,
                        WorkerSurname = worker.Surname
                    }
                )
                .ToListAsync();

            return new RequestHolidaySelectDTO
            {
                RequestHolidays = resultList,
                Count = resultList.Count
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
                    WorkerCf = x.WorkerCf
                })
                .FirstOrDefaultAsync();
        }
    }
}
