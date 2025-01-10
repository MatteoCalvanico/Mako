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
        public List<string> Certificates { get; set; }
        public List<string> Licences { get; set; }
    }

    public class WorkersViewModel
    {
        public List<WorkerViewModel> Workers { get; set; } = new List<WorkerViewModel>();
    }
}

