using Mako.Services;
using Mako.Services.Shared;
using Mako.Services.Shared.Enums;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Mako.Infrastructure
{
    public class DataGenerator
    {
        /// <summary>
        /// Initialize the database with some data
        /// @param context: the database context
        /// </summary>
        public static void Initialize(MakoDbContext context)
        {
            // START INIT: Roles, Certifications, Licence
            InitializeRoles(context);
            InitializeCertifications(context);
            InitializeLicence(context);
            // END INIT: Roles, Certifications, Licence

            // START INIT: Workers, Users
            InitializeWorkers(context);
            InitializeUsers(context);
            // END INIT: Workers, Users

            // START INIT: Ships. Shifts
            InitializeShips(context);
            InitializeShift(context);
            // END INIT: Ships, Shifts

            // START INIT: Requests
            InitializeRequests(context);
            // END INIT: Requests

            // START INIT: JOIN Tables
            InitializeWorkerRoles(context);
            InitializeShiftWorkers(context);
            InitializeJoinCertification(context);
            InitializeJoinLicence(context);
            // END INIT: JOIN Tables
        }


        private static void InitializeRoles(MakoDbContext context)
        {
            if (context.Roles.Any())
            {
                return;   // Data was already seeded
            }
            context.Roles.AddRange(
                new Role
                {
                    Type = RoleTypes.Worker,
                },
                new Role
                {
                    Type = RoleTypes.CraneOperator,
                },
                new Role
                {
                    Type = RoleTypes.ShiftAdmin,
                },
                new Role
                {
                    Type = RoleTypes.Driver,
                },
                new Role
                {
                    Type = RoleTypes.None,
                });
            context.SaveChanges();
        }

        public static void InitializeCertifications(MakoDbContext context)
        {
            if (context.Certifications.Any())
            {
                return;   // Data was already seeded
            }
            context.Certifications.AddRange(
                new Certification
                {
                    Types = CertificationTypes.Explosives,
                },
                new Certification
                {
                    Types = CertificationTypes.Weapons,
                },
                new Certification
                {
                    Types = CertificationTypes.Chemicals,
                },
                new Certification
                {
                    Types = CertificationTypes.None,
                });
            context.SaveChanges();
        }

        private static void InitializeLicence(MakoDbContext context)
        {
            if (context.Licences.Any())
            {
                return;   // Data was already seeded
            }
            context.Licences.AddRange(
                new Licence
                {
                    Types = LicenceTypes.A,
                },
                new Licence
                {
                    Types = LicenceTypes.A1,
                },
                new Licence
                {
                    Types = LicenceTypes.A2,
                },
                new Licence
                {
                    Types = LicenceTypes.AM,
                },
                new Licence
                {
                    Types = LicenceTypes.B,
                },
                new Licence
                {
                    Types = LicenceTypes.B1,
                },
                new Licence
                {
                    Types = LicenceTypes.BE,
                },
                new Licence
                {
                    Types = LicenceTypes.C,
                },
                new Licence
                {
                    Types = LicenceTypes.C1,
                },
                new Licence
                {
                    Types = LicenceTypes.CE,
                },
                new Licence
                {
                    Types = LicenceTypes.C1E,
                },
                new Licence
                {
                    Types = LicenceTypes.D,
                },
                new Licence
                {
                    Types = LicenceTypes.D1,
                },
                new Licence
                {
                    Types = LicenceTypes.DE,
                },
                new Licence
                {
                    Types = LicenceTypes.D1E,
                },
                new Licence
                {
                    Types = LicenceTypes.K,
                },
                new Licence
                {
                    Types = LicenceTypes.None,
                });

        }

        private static void InitializeWorkers(MakoDbContext context)
        {
            if (context.Workers.Any())
            {
                return;   // Data was already seeded
            }

            context.Workers.AddRange(
                new Worker
                {
                    Cf = "00000000001",
                    Name = "admin",
                    Surname = "admin",

                },
                new Worker
                {
                    Cf = "PPPFNC80A01H501K",
                    Name = "Pippo",
                    Surname = "Franco",

                },
                new Worker
                {
                    Cf = "MMMRRRSS03463023",
                    Name = "Mario",
                    Surname = "Rossi",

                },
                new Worker
                {
                    Cf = "JJJDDDOOEH023403",
                    Name = "John",
                    Surname = "Doe",

                },
                new Worker
                {
                    Cf = "GGGFFFERDE034034",
                    Name = "Gordon",
                    Surname = "Freeman",

                });
            context.SaveChanges();
        }

        private static void InitializeUsers(MakoDbContext context)
        {
            if (context.Users.Any())
            {
                return;   // Data was already seeded
            }

            // Passwords need to be converted in SHA-256 base64.
            context.Users.AddRange(
                new User
                {
                    Id = Guid.Parse("3de6883f-9a0b-4667-aa53-0fbc52c4d300"), // Forced to specific Guid for tests
                    Email = "pippo.franco@mako.it",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Cf = "PPPFNC80A01H501K",
                },
                new User
                {
                    Id = Guid.Parse("a030ee81-31c7-47d0-9309-408cb5ac0ac7"), // Forced to specific Guid for tests
                    Email = "admin",
                    Password = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // SHA-256 of text "admin"
                    Cf = "00000000001",
                },
                new User
                {
                    Id = Guid.NewGuid(), 
                    Email = "mario.rossi@mako.it",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Cf = "MMMRRRSS03463023",
                },
                new User
                {
                    Id = Guid.NewGuid(), 
                    Email = "john.doe@mako.it",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Cf = "JJJDDDOOEH023403",
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "gordon.freeman@mako.it",
                    Password = "M0Cuk9OsrcS/rTLGf5SY6DUPqU2rGc1wwV2IL88GVGo=", // SHA-256 of text "Prova"
                    Cf = "GGGFFFERDE034034",
                });

            context.SaveChanges();
        }

        private static void InitializeShips(MakoDbContext context)
        {
            if (context.Ships.Any())
            {
                return;   // Data was already seeded
            }
            context.Ships.AddRange(
                new Ship
                {
                    Name = "Going Merry",
                    DateArrival = DateTime.Now.AddDays(new Random().Next(1, 30)), // Random date between now and 30 days from now
                    DateDeparture = DateTime.Now.AddDays(new Random().Next(31, 60)), // Random date between 31 and 60 days from now
                    Pier = 1,
                    TimeEstimation = TimeSpan.FromHours(10), // 10 hours
                    CargoManifest = "TODO",
                },
                new Ship
                {
                    Name = "Thousand Sunny",
                    DateArrival = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    DateDeparture = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Pier = 2,
                    TimeEstimation = TimeSpan.FromHours(15),
                    CargoManifest = "TODO",
                },
                new Ship
                {
                    Name = "Nautilus",
                    DateArrival = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    DateDeparture = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Pier = 3,
                    TimeEstimation = TimeSpan.FromHours(15),
                    CargoManifest = "TODO",
                },
                new Ship
                {
                    Name = "Over the Rainbow",
                    DateArrival = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    DateDeparture = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Pier = 4,
                    TimeEstimation = TimeSpan.FromHours(15),
                    CargoManifest = "TODO",
                },
                new Ship
                {
                    Name = "Filomons",
                    DateArrival = DateTime.Now,
                    DateDeparture = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Pier = 2,
                    TimeEstimation = TimeSpan.FromHours(15),
                    CargoManifest = "TODO",
                },
                new Ship
                {
                    Name = "Flying Dutchman",
                    DateArrival = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    DateDeparture = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Pier = 2,
                    TimeEstimation = TimeSpan.FromHours(15),
                    CargoManifest = "TODO",
                });
            context.SaveChanges();
        }

        public static void InitializeShift(MakoDbContext context)
        {
            if (context.Shifts.Any())
            {
                return;   // Data was already seeded
            }
            context.Shifts.AddRange(
                new Shift
                {
                    Id = new Guid("3de6883f-9a0b-4667-aa53-0fbc52c4d300"), // Forced to specific Guid for tests
                    Pier = 1,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    StartHour = TimeOnly.FromDateTime(DateTime.Now),
                    EndHour = TimeOnly.FromDateTime(DateTime.Now.AddHours(8)),
                    ShipName = "Going Merry",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Going Merry").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("a030ee81-31c7-47d0-9309-408cb5ac0ac7"), // Forced to specific Guid for tests
                    Pier = 2,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    StartHour = TimeOnly.FromDateTime(DateTime.Now.AddHours(9)),
                    EndHour = TimeOnly.FromDateTime(DateTime.Now.AddHours(3)),
                    ShipName = "Thousand Sunny",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Thousand Sunny").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("a030993f-9a0b-4667-aa53-0fbc52c4d300"), // Forced to specific Guid for tests
                    Pier = 1,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    StartHour = new TimeOnly(7, 30),
                    EndHour = new TimeOnly(12, 30),
                    ShipName = "Going Merry",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Going Merry").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("993faa81-31c7-47d0-9309-408cb5ac0ac7"), // Forced to specific Guid for tests
                    Pier = 2,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    StartHour = new TimeOnly(14, 30),
                    EndHour = new TimeOnly(17, 30),
                    ShipName = "Thousand Sunny",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Thousand Sunny").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("3de6993f-9a0b-4667-31c7-0fbc52c4d300"), // Forced to specific Guid for tests
                    Pier = 1,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                    StartHour = new TimeOnly(7, 30),
                    EndHour = new TimeOnly(12, 30),
                    ShipName = "Going Merry",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Going Merry").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("a030aa81-4667-47d0-9309-0fbc52c4d300"), // Forced to specific Guid for tests
                    Pier = 2,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                    StartHour = new TimeOnly(14, 30),
                    EndHour = new TimeOnly(17, 30),
                    ShipName = "Thousand Sunny",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Thousand Sunny").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("b030aa81-4667-47d0-9309-0fbc52c4d301"), // Forced to specific Guid for tests
                    Pier = 1,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
                    StartHour = new TimeOnly(8, 0),
                    EndHour = new TimeOnly(12, 0),
                    ShipName = "Going Merry",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Going Merry").DateArrival,
                },
                new Shift
                {
                    Id = new Guid("c030aa81-4667-47d0-9309-0fbc52c4d302"), // Forced to specific Guid for tests
                    Pier = 2,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
                    StartHour = new TimeOnly(13, 0),
                    EndHour = new TimeOnly(17, 0),
                    ShipName = "Thousand Sunny",
                    ShipDateArrival = context.Ships.First(s => s.Name == "Thousand Sunny").DateArrival,
                }
            );
            context.SaveChanges();
        }

        private static void InitializeWorkerRoles(MakoDbContext context)
        {
            if (context.WorkerRoles.Any())
            {
                return;   // Data was already seeded
            }

            var worker1 = context.Workers.First(w => w.Cf == "00000000001");
            var worker2 = context.Workers.First(w => w.Cf == "PPPFNC80A01H501K");
            var worker3 = context.Workers.First(w => w.Cf == "MMMRRRSS03463023");
            var worker4 = context.Workers.First(w => w.Cf == "JJJDDDOOEH023403");
            var worker5 = context.Workers.First(w => w.Cf == "GGGFFFERDE034034");

            var roleWorker = context.Roles.First(r => r.Type == RoleTypes.Worker);
            var roleCraneOp = context.Roles.First(r => r.Type == RoleTypes.CraneOperator);
            var roleShiftAdmin = context.Roles.First(r => r.Type == RoleTypes.ShiftAdmin);
            var roleDriver = context.Roles.First(r => r.Type == RoleTypes.Driver);
            var roleNone = context.Roles.First(r => r.Type == RoleTypes.None);

            context.WorkerRoles.AddRange(
                new WorkerRole { WorkerCf = worker1.Cf, RoleId = roleWorker.Id },
                new WorkerRole { WorkerCf = worker1.Cf, RoleId = roleShiftAdmin.Id },
                new WorkerRole { WorkerCf = worker2.Cf, RoleId = roleCraneOp.Id },
                new WorkerRole { WorkerCf = worker2.Cf, RoleId = roleWorker.Id },
                new WorkerRole { WorkerCf = worker3.Cf, RoleId = roleDriver.Id },
                new WorkerRole { WorkerCf = worker3.Cf, RoleId = roleWorker.Id },
                new WorkerRole { WorkerCf = worker4.Cf, RoleId = roleWorker.Id },
                new WorkerRole { WorkerCf = worker5.Cf, RoleId = roleWorker.Id }
            );

            context.SaveChanges();
        }

        private static void InitializeShiftWorkers(MakoDbContext context)
        {
            if (context.ShiftWorker.Any())
            {
                return;   // Data was already seeded
            }

            var shift1 = context.Shifts.First(s => s.Id == new Guid("3de6883f-9a0b-4667-aa53-0fbc52c4d300"));
            var shift2 = context.Shifts.First(s => s.Id == new Guid("a030ee81-31c7-47d0-9309-408cb5ac0ac7"));
            var shift3 = context.Shifts.First(s => s.Id == new Guid("a030993f-9a0b-4667-aa53-0fbc52c4d300"));
            var shift4 = context.Shifts.First(s => s.Id == new Guid("993faa81-31c7-47d0-9309-408cb5ac0ac7"));
            var shift5 = context.Shifts.First(s => s.Id == new Guid("3de6993f-9a0b-4667-31c7-0fbc52c4d300"));
            var shift6 = context.Shifts.First(s => s.Id == new Guid("a030aa81-4667-47d0-9309-0fbc52c4d300"));
            var shift7 = context.Shifts.First(s => s.Id == new Guid("b030aa81-4667-47d0-9309-0fbc52c4d301"));
            var shift8 = context.Shifts.First(s => s.Id == new Guid("c030aa81-4667-47d0-9309-0fbc52c4d302"));

            var worker1 = context.Workers.First(w => w.Cf == "00000000001");
            var worker2 = context.Workers.First(w => w.Cf == "PPPFNC80A01H501K");

            context.ShiftWorker.AddRange(
                new ShiftWorker { ShiftId = shift1.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift1.Id, WorkerCf = worker2.Cf },
                new ShiftWorker { ShiftId = shift2.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift2.Id, WorkerCf = worker2.Cf },
                new ShiftWorker { ShiftId = shift3.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift4.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift5.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift6.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift7.Id, WorkerCf = worker1.Cf },
                new ShiftWorker { ShiftId = shift8.Id, WorkerCf = worker1.Cf }
            );

            context.SaveChanges();
        }

        private static void InitializeRequests(MakoDbContext context)
        {
            if (context.RequestsHolidays.Any())
            {
                return;   // Data was already seeded
            }
            if (context.RequestsChanges.Any())
            {
                return;   // Data was already seeded
            }
            context.RequestsHolidays.AddRange(
                new RequestHoliday
                {
                    WorkerCf = "00000000001",
                    Id = Guid.Parse("3de6883f-9a0b-4667-aa53-0fbc52c4d300"), // Forced to specific Guid for tests
                    SentDate = DateTime.Now.AddDays(new Random().Next(-30, 0)),
                    StartDate = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    EndDate = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Motivation = "I need some time off",
                    State = RequestState.Accepted,
                },
                new RequestHoliday
                {
                    WorkerCf = "PPPFNC80A01H501K",
                    Id = Guid.Parse("a030ee81-31c7-47d0-9309-408cb5ac0ac7"), // Forced to specific Guid for tests
                    SentDate = DateTime.Now.AddDays(new Random().Next(-30, 0)),
                    StartDate = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    EndDate = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Motivation = "I need some time off",
                    State = RequestState.Unmanaged,
                },
                new RequestHoliday
                {
                    WorkerCf = "PPPFNC80A01H501K",
                    Id = Guid.Parse("0fbc52c4-9309-47d0-31c7-3de6883fac74"), // Forced to specific Guid for tests
                    SentDate = DateTime.Now.AddDays(new Random().Next(-30, 0)),
                    StartDate = DateTime.Now.AddDays(new Random().Next(1, 30)),
                    EndDate = DateTime.Now.AddDays(new Random().Next(31, 60)),
                    Motivation = "I need some time off",
                    State = RequestState.Declined,
                });
            context.RequestsChanges.AddRange(
                new RequestChange
                {
                    WorkerCf = "00000000001",
                    Id = Guid.Parse("3de6883f-9a0b-4667-aa53-0fbc30d4a500"), // Forced to specific Guid for tests
                    SentDate = DateTime.Now.AddDays(new Random().Next(-30, 0)),
                    Motivation = "I'm sick",
                    State = RequestState.Accepted,
                    ShiftId = Guid.Parse("3de6883f-9a0b-4667-aa53-0fbc52c4d300"),
                },
                new RequestChange
                {
                    WorkerCf = "PPPFNC80A01H501K",
                    Id = Guid.Parse("a030ee81-4667-47d0-9309-0fbc52c4d300"), // Forced to specific Guid for tests
                    SentDate = DateTime.Now.AddDays(new Random().Next(-30, 0)),
                    Motivation = "I'm sick",
                    State = RequestState.Accepted,
                    ShiftId = Guid.Parse("a030ee81-31c7-47d0-9309-408cb5ac0ac7"),
                });
            context.SaveChanges();
        }

        private static void InitializeJoinCertification(MakoDbContext context)
        {
            if (context.JoinCertifications.Any())
            {
                return;   // Data was already seeded
            }
            var worker1 = context.Workers.First(w => w.Cf == "00000000001");
            var worker2 = context.Workers.First(w => w.Cf == "PPPFNC80A01H501K");
            var worker3 = context.Workers.First(w => w.Cf == "MMMRRRSS03463023");
            var worker4 = context.Workers.First(w => w.Cf == "JJJDDDOOEH023403");
            var worker5 = context.Workers.First(w => w.Cf == "GGGFFFERDE034034");

            var certificationExplosives = context.Certifications.First(c => c.Types == CertificationTypes.Explosives);
            var certificationWeapons = context.Certifications.First(c => c.Types == CertificationTypes.Weapons);
            var certificationChemicals = context.Certifications.First(c => c.Types == CertificationTypes.Chemicals);
            var certificationNone = context.Certifications.First(c => c.Types == CertificationTypes.None);
            var random = new Random();
            context.JoinCertifications.AddRange(
                new JoinCertification { WorkerCf = worker1.Cf, CertificationId = certificationExplosives.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) },
                new JoinCertification { WorkerCf = worker1.Cf, CertificationId = certificationWeapons.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) },
                new JoinCertification { WorkerCf = worker2.Cf, CertificationId = certificationNone.Id },
                new JoinCertification { WorkerCf = worker3.Cf, CertificationId = certificationChemicals.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) },
                new JoinCertification { WorkerCf = worker4.Cf, CertificationId = certificationExplosives.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) },
                new JoinCertification { WorkerCf = worker5.Cf, CertificationId = certificationWeapons.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) }
            );
            context.SaveChanges();
        }

        private static void InitializeJoinLicence(MakoDbContext context)
        {
            if (context.JoinLicences.Any())
            {
                return;   // Data was already seeded
            }
            var worker1 = context.Workers.First(w => w.Cf == "00000000001");
            var worker2 = context.Workers.First(w => w.Cf == "PPPFNC80A01H501K");
            var licenceA = context.Licences.First(l => l.Types == LicenceTypes.A);
            var licenceA1 = context.Licences.First(l => l.Types == LicenceTypes.A1);
            var licenceA2 = context.Licences.First(l => l.Types == LicenceTypes.A2);
            var licenceNone = context.Licences.First(l => l.Types == LicenceTypes.None);
            var random = new Random();
            context.JoinLicences.AddRange(
                new JoinLicence { WorkerCf = worker1.Cf, LicenceId = licenceA.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 6))) },
                new JoinLicence { WorkerCf = worker1.Cf, LicenceId = licenceA1.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) },
                new JoinLicence { WorkerCf = worker1.Cf, LicenceId = licenceA2.Id, ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 365))) }
                // new JoinLicence { WorkerCf = worker2.Cf, LicenceId = licenceNone.Id }
            );
            context.SaveChanges();
        }
    }
}
