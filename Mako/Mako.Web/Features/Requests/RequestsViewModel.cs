using Mako.Services.Shared;
using System;
using System.Collections.Generic;

namespace Mako.Web.Features.Requests
{
    /// <summary>
    /// View model for a request, is a mix between Change and Holidays requests.
    /// 
    /// <para>Leave null the properties that are not present in the request type you are working with.
    /// Then in the view you can check if the property is null to show or hide the information.</para>
    /// </summary>
    public class RequestViewModel
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ShiftDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Motivation { get; set; }
        public int Pier { get; set; }
        public RequestState State { get; set; }
        public string WorkerCf { get; set; }
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
        public List<RequestChangeCustom> ChangeRequests { get; set; } = new List<RequestChangeCustom>();
        public List<RequestHolidayCustom> HolidayRequests { get; set; } = new List<RequestHolidayCustom>();
    }

    public class RequestChangeCustom
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

    public class RequestHolidayCustom
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