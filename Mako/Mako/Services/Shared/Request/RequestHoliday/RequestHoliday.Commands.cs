using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services.Shared
{
    public class AddOrUpdateRequestHolidayCommand
    {
        public Guid? Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Motivation { get; set; }
        [Required]
        public string Sender { get; set; }
        public RequestState State { get; set; } = RequestState.Unmanaged;
    }

    public partial class SharedService
    {
        public async Task<Guid> Handle(AddOrUpdateRequestHolidayCommand cmd)
        {
            var requestHoliday = await _dbContext.RequestsHolidays
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();

            if (requestHoliday == null)
            {
                requestHoliday = new RequestHoliday
                {
                    StartDate = cmd.StartDate,
                    EndDate = cmd.EndDate,
                    Motivation = cmd.Motivation,
                    Sender = cmd.Sender,
                    State = cmd.State,
                    SentDate = DateTime.UtcNow
                };
                _dbContext.RequestsHolidays.Add(requestHoliday);
            }
            else
            {
                requestHoliday.StartDate = cmd.StartDate;
                requestHoliday.EndDate = cmd.EndDate;
                requestHoliday.Motivation = cmd.Motivation;
                requestHoliday.Sender = cmd.Sender;
                requestHoliday.State = cmd.State;
            }

            await _dbContext.SaveChangesAsync();

            return requestHoliday.Id;
        }
    }
}
