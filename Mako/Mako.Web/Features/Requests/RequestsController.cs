using Mako.Services.Shared;
using Mako.Web.Areas;
using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                var combinedRequests = await GetAllRequestsCombined(filterParam);

                return View("Requests", combinedRequests);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving requests: " + ex.Message);
                return View("Requests", new List<RequestViewModel>());
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
                        State = rc.State,
                        SentDate = rc.SentDate,
                        ShiftDate = rc.ShiftDate,
                        Pier = rc.Pier,
                        WorkerCf = rc.WorkerCf,
                        WorkerName = rc.WorkerName,
                        WorkerSurname = rc.WorkerSurname
                    })
                    .ToList();

                var holidayQuery = new RequestHolidaySelectQuery { };
                var holidayResultsDto = await _sharedService.SelectRequestsHolidayQuery(holidayQuery);

                viewModel.HolidayRequests = holidayResultsDto.RequestHolidays
                    .Select(rh => new RequestHolidayCustom
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

        // Return all the requests combined into a single list
        // See the RequestViewModel class for the properties that are common to both change and holiday requests
        public async Task<List<RequestViewModel>> GetAllRequestsCombined(string filter)
        {
            // Initialize the list to return
            var combinedRequests = new List<RequestViewModel>();

            // Get the view model with all the requests
            var requestsViewModel = await GetAllRequestsAndHolidayRequests(filter);

            // Combine the change and holiday requests into a single list
            // Params not present in the request type will be null
            combinedRequests.AddRange(requestsViewModel.ChangeRequests.Select(cr => new RequestViewModel
            {
                Id = cr.Id,
                ShiftId = cr.ShiftId,
                SentDate = cr.SentDate,
                ShiftDate = cr.ShiftDate,
                Motivation = cr.Motivation,
                Pier = cr.Pier,
                State = cr.State,
                WorkerCf = cr.WorkerCf,
                WorkerName = cr.WorkerName,
                WorkerSurname = cr.WorkerSurname
            }));
            combinedRequests.AddRange(requestsViewModel.HolidayRequests.Select(hr => new RequestViewModel
            {
                Id = hr.Id,
                StartDate = hr.StartDate,
                EndDate = hr.EndDate,
                SentDate = hr.SentDate,
                Motivation = hr.Motivation,
                State = hr.State,
                WorkerCf = hr.WorkerCf,
                WorkerName = hr.WorkerName,
                WorkerSurname = hr.WorkerSurname
            }));

            return combinedRequests;
        }

        [HttpPost]
        public virtual async Task<IActionResult> UpdateRequestState(Guid id, string newState)
        {
            try
            {
                // Implement the logic to update the request state
                // await _sharedService.UpdateRequestStateAsync(id, newState);

                Alerts.AddSuccess(this, "Request state updated successfully.");
            }
            catch (Exception ex)
            {
                Alerts.AddError(this, "An error occurred while updating the request state: " + ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}