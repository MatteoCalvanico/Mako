﻿using Mako.Infrastructure;
using Mako.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services
{
    public class MakoDbContext : DbContext
    {
        public MakoDbContext()
        {
        }

        public MakoDbContext(DbContextOptions<MakoDbContext> options) : base(options)
        {
            DataGenerator.InitializeUsers(this);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<RequestHoliday> RequestsHolidays { get; set; }
        public DbSet<RequestChange> RequestsChanges { get; set; }
        public DbSet<Certification> Certifications{ get; set; }
        public DbSet<JoinCertification> JoinCertifications { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
