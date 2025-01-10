using Microsoft.AspNetCore.Mvc;
using Mako.Services.Shared;
using Mako.Web.Areas;
using Mako.Web.Features.Workers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;

namespace Mako.Web.Features.Workers
{
    public partial class WorkersController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public WorkersController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index()
        {
            // Recupera il ViewModel completo (lista di WorkerViewModel)
            var workersViewModel = await GetAllWorkers();
            return View("Index", workersViewModel);
        }

        public async Task<WorkersViewModel> GetAllWorkers()
        {
            var viewModel = new WorkersViewModel();

            try
            {
                var query = new WorkersComplexQuery
                {
                    Filter = ""
                };

                // Richiamiamo il tuo metodo in Worker.Queries.cs
                // che fa la catena di join e ritorna roles, cert, licence.
                var workersDTO = await _sharedService.SelectWorkersComplex(query);
                // Mappiamo i DTO in WorkerViewModel
                viewModel.Workers = workersDTO.Workers
                    .Select(w => new WorkerViewModel
                    {
                        Cf = w.Cf,
                        Name = w.Name,
                        Surname = w.Surname,
                        Roles = w.Roles,
                        Certificates = w.Certificates,
                        Licences = w.Licences
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all workers", ex);
            }

            return viewModel;
        }
    }
}
