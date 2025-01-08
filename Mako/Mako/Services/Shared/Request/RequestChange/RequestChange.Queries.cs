using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services.Shared
{
    public class RequestChangeSelectQuery
    {
        public Guid? Id { get; set; }
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
            public string WorkerCf { get; set; }
            public DateTime ShiftDate { get; set; }
            public int Pier { get; set; }
            public string WorkerName { get; set; }
            public string WorkerSurname { get; set; }
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
        public Worker Sender { get; set; }
    }

    public partial class SharedService
    {
        public async Task<RequestChangeSelectDTO> SelectRequestChange(RequestChangeSelectQuery qry)
        {
            var changeQuery = _dbContext.RequestsChanges.AsQueryable();

            // Apply filters if any
            if (qry.Id.HasValue)
            {
                changeQuery = changeQuery.Where(x => x.Id == qry.Id.Value);
            }
            if (!string.IsNullOrEmpty(qry.Filter))
            {
                changeQuery = changeQuery.Where(x => x.Motivation.Contains(qry.Filter));
            }

            var resultList = await changeQuery
                .Join(_dbContext.Shifts,
                    change => change.ShiftId,
                    shift => shift.Id,
                    (change, shift) => new { change, shift }
                )
                .Join(_dbContext.Workers,
                    cs => cs.change.WorkerCf,
                    worker => worker.Cf,
                    (cs, worker) => new RequestChangeSelectDTO.RequestChange
                    {
                        Id = cs.change.Id,
                        ShiftId = cs.change.ShiftId,
                        Motivation = cs.change.Motivation,
                        State = cs.change.State,
                        SentDate = cs.change.SentDate,
                        WorkerCf = cs.change.WorkerCf,
                        ShiftDate = cs.shift.Date.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                        Pier = cs.shift.Pier,
                        WorkerName = worker.Name,
                        WorkerSurname = worker.Surname
                    }
                )
                .ToListAsync();

            return new RequestChangeSelectDTO
            {
                RequestChanges = resultList,
                Count = resultList.Count()
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