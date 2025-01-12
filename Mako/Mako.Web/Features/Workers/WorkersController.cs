using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mako.Services.Shared;
using Mako.Web.Areas;

namespace Mako.Web.Features.Workers
{
    public partial class WorkersController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public WorkersController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index(string searchTerm = null, string filterType = null)
        {
            var workersViewModel = await GetAllWorkers();

            // Basic filtering logic example:
            if (!string.IsNullOrEmpty(searchTerm))
            {
                switch (filterType)
                {
                    case "Name":
                        workersViewModel.Workers = workersViewModel.Workers
                            .Where(w => (w.Name + " " + w.Surname)
                            .Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "Ruolo":
                        workersViewModel.Workers = workersViewModel.Workers
                            .Where(w => w.Roles.Any(r => r.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                        break;

                    case "Licence":
                        workersViewModel.Workers = workersViewModel.Workers
                            .Where(w => w.Licences.Any(l => l.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                        break;

                    case "Certification":
                        workersViewModel.Workers = workersViewModel.Workers
                            .Where(w => w.Certificates.Any(c => c.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                        break;

                    default:
                        // Fallback: match name, roles, certificates, or licences
                        workersViewModel.Workers = workersViewModel.Workers
                            .Where(w =>
                                (w.Name + " " + w.Surname)
                                .Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                || w.Roles.Any(r => r.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                || w.Certificates.Any(c => c.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                || w.Licences.Any(l => l.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                            )
                            .ToList();
                        break;
                }
            }

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

                var workersDTO = await _sharedService.SelectWorkersComplex(query);

                viewModel.Workers = workersDTO.Workers
                    .Select(w => new WorkerViewModel
                    {
                        Cf = w.Cf,
                        Name = w.Name,
                        Surname = w.Surname,
                        Roles = w.Roles,
                        Certificates = w.Certifications.Select(c => new CustomLicenceCertificate
                        {
                            Type = c.Type,
                            ExpiryDate = c.ExpireDate
                        }).ToList(),
                        Licences = w.Licences.Select(l => new CustomLicenceCertificate
                        {
                            Type = l.Type,
                            ExpiryDate = l.ExpireDate
                        }).ToList()
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