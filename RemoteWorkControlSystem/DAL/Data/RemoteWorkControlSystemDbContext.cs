using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class RemoteWorkControlSystemDbContext : DbContext
    {

        public RemoteWorkControlSystemDbContext(DbContextOptions<RemoteWorkControlSystemDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<EmployeeScreenActivity> EmployeeScreenActivities { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskDuration> TaskDurations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


    }
}
