using Mako.Services.Shared;
using System;
using System.Collections.Generic;

namespace Mako.Web.Features.Requests
{
    public class RequestViewModel
    {
        public Guid Id { get; set; }
        public DateTime SentDate { get; set; }
        public string Motivation { get; set; }
        public RequestState State { get; set; }
        public string WorkerCf { get; set; }
        public Worker Sender { get; set; }
        public string WorkerName { get; set; }
        public string WorkerSurname { get; set; }
    }

    public enum RequestState
    {
        Accepted,
        Declined,
        Unmanaged
    }

    public class RequestsViewModel
    {
        public List<RequestChange> ChangeRequests { get; set; } = new List<RequestChange>();
        public List<RequestHoliday> HolidayRequests { get; set; } = new List<RequestHoliday>();
    }
}