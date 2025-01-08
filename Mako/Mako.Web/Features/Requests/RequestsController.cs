using Mako.Services.Shared;
using Mako.Web.Areas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Web.Features.Requests
{
    public partial class RequestsController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public RequestsController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        // Example action that retrieves both change requests and holiday requests
        public virtual async Task<IActionResult> Index()
        {
            try
            {
                // Example: retrieve some filter value to pass along, if needed
                var filterParam = Request.Query["filter"].ToString();

                // Combine everything into a single view model
                var requestsViewModel = await GetAllRequestsAndHolidayRequests(filterParam);

                return View("Requests", requestsViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving requests: " + ex.Message);
                return View("Requests", new RequestsViewModel());
            }
        }

        // Main method that runs queries for changes and holidays, maps them into a RequestsViewModel
        public async Task<RequestsViewModel> GetAllRequestsAndHolidayRequests(string filter)
        {
            var viewModel = new RequestsViewModel();

            try
            {
                var changesQuery = new RequestChangeSelectQuery
                {
                    Filter = filter
                };
                var changeResultsDto = await _sharedService.SelectRequestChange(changesQuery);

                viewModel.ChangeRequests = changeResultsDto.RequestChanges
                    .Select(rc => new RequestChangeCustom
                    {
                        Id = rc.Id,
                        ShiftId = rc.ShiftId,
                        Motivation = rc.Motivation,
                        State = (RequestState)rc.State,
                        SentDate = rc.SentDate,
                        ShiftDate = rc.ShiftDate,
                        Pier = rc.Pier,
                        WorkerCf = rc.WorkerCf,
                        WorkerName = rc.WorkerName,
                        WorkerSurname = rc.WorkerSurname
                    })
                    .ToList();

                foreach (var changeRequest in viewModel.ChangeRequests)
                {
                    var worker = await _sharedService.Query(new WorkersDetailQuery { Cf = changeRequest.WorkerCf });
                    changeRequest.WorkerName = worker?.Name ?? string.Empty;
                    changeRequest.WorkerSurname = worker?.Surname ?? string.Empty;
                }

                var holidayQuery = new RequestHolidaySelectQuery { };
                var holidayResultsDto = await _sharedService.SelectRequestsHolidayQuery(holidayQuery);

                viewModel.HolidayRequests = holidayResultsDto.RequestHolidays
                    .Select(rh => new RequestHolidayCustom
                    {
                        Id = rh.Id,
                        StartDate = rh.StartDate,
                        EndDate = rh.EndDate,
                        Motivation = rh.Motivation ?? string.Empty,
                        State = (RequestState)rh.State,
                        SentDate = rh.SentDate,
                        WorkerCf = rh.WorkerCf ?? string.Empty,
                        WorkerName = rh.WorkerName ?? string.Empty,
                        WorkerSurname = rh.WorkerSurname ?? string.Empty
                    })
                    .ToList();

                foreach (var holidayRequest in viewModel.HolidayRequests)
                {
                    var worker = await _sharedService.Query(new WorkersDetailQuery { Cf = holidayRequest.WorkerCf });
                    holidayRequest.WorkerName = worker?.Name ?? string.Empty;
                    holidayRequest.WorkerSurname = worker?.Surname ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as required
                throw new Exception("An error occurred while retrieving requests and holiday requests", ex);
            }

            return viewModel;
        }
    }
}