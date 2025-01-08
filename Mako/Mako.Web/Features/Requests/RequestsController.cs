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
                    .Select(rc => new RequestChange
                    {
                        Id = rc.Id,
                        ShiftId = rc.ShiftId,
                        Motivation = rc.Motivation,
                        State = rc.State,
                        SentDate = rc.SentDate
                    })
                    .ToList();

                // Retrieve holiday requests
                var holidayQuery = new RequestHolidaySelectQuery{};
                var holidayResultsDto = await _sharedService.SelectRequestsHolidayQuery(holidayQuery);

                viewModel.HolidayRequests = holidayResultsDto.RequestHolidays
                    .Select(rh => new RequestHoliday
                    {
                        Id = rh.Id,
                        StartDate = rh.StartDate,
                        EndDate = rh.EndDate,
                        Motivation = rh.Motivation,
                        State = rh.State,
                        SentDate = rh.SentDate,
                        WorkerCf = rh.WorkerCf,
                        WorkerName = rh.WorkerName,
                        WorkerSurname = rh.WorkerSurname
                    })
                    .ToList();
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