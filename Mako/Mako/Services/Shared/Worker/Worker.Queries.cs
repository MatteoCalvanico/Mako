using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        }
    }

    public class WorkersDetailQuery
    {
        public string Cf {  get; set; }
    }

    public class WorkersDetailDTO
    {
        public string Cf { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Certificates { get; set; }
        public List<string> Licences { get; set; }
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
                .Where(x => x.Cf != qry.IdCurrentWorker);

            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Cf.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new WorkersSelectDTO
            {
                Workers = await queryable
                .Select(x => new WorkersSelectDTO.Worker
                {
                    cf = x.Cf,
                    name = x.Name,
                    surname = x.Surname,
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
        public async Task<WorkersDetailDTO> QueryWorkerDetail(WorkersDetailQuery qry)
        {
            return await _dbContext.Workers
                .Where(x => x.Cf == qry.Cf)
                .Select(x => new WorkersDetailDTO
                {
                    Cf = x.Cf,
                    Name = x.Name,
                    Surname = x.Surname,
                })
                .FirstOrDefaultAsync();
        }
    }



    public class WorkersComplexQuery
    {
        public string Filter { get; set; }
    }

    public class WorkersComplexDTO
    {
        public IEnumerable<WorkerDTO> Workers { get; set; }
        public int Count { get; set; }

        public class WorkerDTO
        {
            public string Cf { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }

            //liste per ruoli, certificati e licenze
            public List<string> Roles { get; set; }
            public List<string> Certificates { get; set; }
            public List<string> Licences { get; set; }
        }
    }


        public partial class SharedService
    {
        public async Task<WorkersComplexDTO> SelectWorkersComplex(WorkersComplexQuery qry)
        {
            // Step 1: Retrieve all workers based on the filter
            var baseQuery = _dbContext.Workers.AsQueryable();

            if (!string.IsNullOrEmpty(qry.Filter))
            {
                baseQuery = baseQuery.Where(w =>
                    w.Cf.Contains(qry.Filter)
                    || w.Name.Contains(qry.Filter)
                    || w.Surname.Contains(qry.Filter)
                );
            }

            var workers = await baseQuery.ToListAsync();

            // Step 2: Retrieve related data for the workers
            var workerCfs = workers.Select(w => w.Cf).ToList();

            var workerRoles = await _dbContext.WorkerRoles
                .Where(wr => workerCfs.Contains(wr.WorkerCf))
                .ToListAsync();

            var roles = await _dbContext.Roles
                .Where(r => workerRoles.Select(wr => wr.RoleId).Contains(r.Id))
                .ToListAsync();

            var joinCertifications = await _dbContext.JoinCertifications
                .Where(jc => workerCfs.Contains(jc.WorkerCf))
                .ToListAsync();

            var certifications = await _dbContext.Certifications
                .Where(c => joinCertifications.Select(jc => jc.CertificationId).Contains(c.Id))
                .ToListAsync();

            var joinLicences = await _dbContext.JoinLicences
                .Where(jl => workerCfs.Contains(jl.WorkerCf))
                .ToListAsync();

            var licences = await _dbContext.Licences
                .Where(l => joinLicences.Select(jl => jl.LicenceId).Contains(l.Id))
                .ToListAsync();

            // Step 3: Assemble the result
            var resultList = (from worker in workers
                              join wr in workerRoles on worker.Cf equals wr.WorkerCf into workerRolesGroup
                              from wr in workerRolesGroup.DefaultIfEmpty()
                              join role in roles on wr?.RoleId equals role.Id into rolesGroup
                              from role in rolesGroup.DefaultIfEmpty()
                              join jc in joinCertifications on worker.Cf equals jc.WorkerCf into joinCertificationsGroup
                              from jc in joinCertificationsGroup.DefaultIfEmpty()
                              join cert in certifications on jc?.CertificationId equals cert.Id into certificationsGroup
                              from cert in certificationsGroup.DefaultIfEmpty()
                              join jl in joinLicences on worker.Cf equals jl.WorkerCf into joinLicencesGroup
                              from jl in joinLicencesGroup.DefaultIfEmpty()
                              join licence in licences on jl?.LicenceId equals licence.Id into licencesGroup
                              from licence in licencesGroup.DefaultIfEmpty()
                              select new
                              {
                                  worker.Cf,
                                  worker.Name,
                                  worker.Surname,
                                  RoleType = role?.Type,
                                  CertType = cert?.Types,
                                  LicenceType = licence?.Types
                              }).ToList();

            var grouped = resultList
                .GroupBy(x => new { x.Cf, x.Name, x.Surname })
                .Select(grp => new WorkersComplexDTO.WorkerDTO
                {
                    Cf = grp.Key.Cf,
                    Name = grp.Key.Name,
                    Surname = grp.Key.Surname,
                    Roles = grp
                        .Where(x => x.RoleType != null)
                        .Select(x => x.RoleType.ToString())
                        .Distinct()
                        .ToList(),
                    Certificates = grp
                        .Where(x => x.CertType != null)
                        .Select(x => x.CertType.ToString())
                        .Distinct()
                        .ToList(),
                    Licences = grp
                        .Where(x => x.LicenceType != null)
                        .Select(x => x.LicenceType.ToString())
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return new WorkersComplexDTO
            {
                Workers = grouped,
                Count = grouped.Count
            };
        }
    }
}
