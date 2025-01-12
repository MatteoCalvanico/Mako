using System;
using System.Collections.Generic;

namespace Mako.Web.Features.Workers
{
    public class WorkerViewModel
    {
        public string Cf { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        // Liste per ruoli, certificati e licenze
        public List<string> Roles { get; set; }
        public List<CustomLicenceCertificate> Certificates { get; set; }
        public List<CustomLicenceCertificate> Licences { get; set; }
    }

    public class WorkersViewModel
    {
        public List<WorkerViewModel> Workers { get; set; } = new List<WorkerViewModel>();
    }

    public class CustomLicenceCertificate
    {
        public string Type { get; set; }
        public DateOnly ExpiryDate { get; set; }
    }
}