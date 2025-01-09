﻿using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
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
            var baseQuery = _dbContext.Workers.AsQueryable();

            // filtro che se necessario si può aggiungere cf o id
            if (!string.IsNullOrEmpty(qry.Filter))
            {
                baseQuery = baseQuery.Where(w =>
                    w.Cf.Contains(qry.Filter)
                    || w.Name.Contains(qry.Filter)
                    || w.Surname.Contains(qry.Filter)
                );
            }


            var resultList = await baseQuery
                // 1) Join con WorkerRoles (si collegano Worker a WorkerRole)
                .Join(_dbContext.WorkerRoles,
                    worker => worker.Cf,
                    wr => wr.WorkerCf,
                    (worker, wr) => new { worker, wr })

                //join su Role
                .Join(_dbContext.Roles,
                    combo => combo.wr.RoleId,
                    r => r.Id,
                    (combo, r) => new
                    {
                        combo.worker,
                        combo.wr,
                        role = r    // Sarà { Id, Type = RoleTypes Type }
                    })

                //join su JoinCertification
                .Join(_dbContext.JoinCertifications,
                    combo => combo.worker.Cf,
                    jc => jc.WorkerCf,
                    (combo, jc) => new
                    {
                        combo.worker,
                        combo.wr,
                        combo.role,
                        jc
                    })

                // d) Join su Certification
                .Join(_dbContext.Certifications,
                     combo => combo.jc.CertificationId,
                     cert => cert.Id,
                     (combo, cert) => new
                     {
                         combo.worker,
                         combo.wr,
                         combo.role,
                         combo.jc,
                         cert
                     })


                //join su JoinLicence
                .Join(_dbContext.JoinLicences,
                    combo => combo.worker.Cf,
                    jl => jl.WorkerCf,
                    (combo, jl) => new
                    {
                        combo.worker,
                        combo.wr,
                        combo.role,
                        combo.jc,
                        combo.cert,
                        jl
                    })

                //join su Licence
                .Join(_dbContext.Licences,
                     combo => combo.jl.LicenceId,
                     licence => licence.Id,
                     (combo, licence) => new
                     {
                         Cf = combo.worker.Cf,
                         Name = combo.worker.Name,
                         Surname = combo.worker.Surname,

                         RoleType = combo.role.Type,
                         CertType = combo.cert.Types,
                         LicenceType = licence.Types
                     }
                )

                .ToListAsync();

            var grouped =  resultList
                .GroupBy(x => new { x.Cf, x.Name, x.Surname })
                .Select(grp => new WorkersComplexDTO.WorkerDTO
                {
                    Cf = grp.Key.Cf,
                    Name = grp.Key.Name,
                    Surname = grp.Key.Surname,
                    
                    Roles = grp
                        .Select(x => x.RoleType.ToString())
                        .Distinct()
                        .ToList(),
                    
                    Certificates = grp
                        .Select(x => x.CertType.ToString())
                        .Distinct()
                        .ToList(),

                    Licences = grp
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
