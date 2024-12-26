using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class WorkersSelectQuery
    {
        public string IdCurrentWorker { get; set; }
        public string Filter {  get; set; }
    }

    public class WorkersSelectDTO
    {
        public IEnumerable<Worker> Workers { get; set; }

        public int Count { get; set; }

        public class Worker
        {
            public string cf;

            public string name;

            public string surname;

            public ICollection<Role> roles { get; set; }
        }
    }

    public class WorkersDetailQuery
    {
        public string Cf {  get; set; }
    }

    public class WorkersDetailDTO
    {
        public string cf;

        public string name;

        public string surname;

        public ICollection<Role> roles { get; set; }
    }


    public partial class SharedService
    {
        /// <summary>
        /// Returns workers for a select field
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<WorkersSelectDTO> Query(WorkersSelectQuery qry)
        {
            var queryable = _dbContext.Workers
                .Where(x => x.cf != qry.IdCurrentWorker);

            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.cf.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new WorkersSelectDTO
            {
                Workers = await queryable
                .Select(x => new WorkersSelectDTO.Worker
                {
                    cf = x.cf,
                    name = x.name,
                    surname = x.surname,
                    roles = x.roles
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        /// <summary>
        /// Returns the detail of the worker who matches the Id passed in the qry parameter
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<WorkersDetailDTO> Query(WorkersDetailQuery qry)
        {
            return await _dbContext.Workers
                .Where(x => x.cf == qry.Cf)
                .Select(x => new WorkersDetailDTO
                {
                    cf = x.cf,
                    name = x.name,
                    surname = x.surname,
                    roles = x.roles
                })
                .FirstOrDefaultAsync();
        }
    }
}
