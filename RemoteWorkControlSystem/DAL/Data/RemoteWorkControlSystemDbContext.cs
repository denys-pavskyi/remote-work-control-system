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
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeScreenActivity>()
                .HasOne(x => x.ProjectMember).WithMany(x => x.EmployeeScreenActivities).HasForeignKey(x => x.ProjectMemberId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
                .HasMany(x => x.ProjectMembers).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProjectMember>()
                .HasOne(x => x.User).WithMany(x => x.ProjectMembers).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectMember>()
                .HasOne(x => x.Project).WithMany(x => x.ProjectMembers).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectMember>()
                .HasMany(x => x.TaskDurations).WithOne(x => x.ProjectMember).HasForeignKey(x => x.ProjectMemberId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectMember>()
                .HasMany(x => x.WorkSessions).WithOne(x => x.ProjectMember).HasForeignKey(x => x.ProjectMemberId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectMember>()
                .HasMany(x => x.EmployeeScreenActivities).WithOne(x => x.ProjectMember).HasForeignKey(x => x.ProjectMemberId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<TaskDuration>()
                .HasOne(x => x.ProjectMember).WithMany(x => x.TaskDurations).HasForeignKey(x => x.ProjectMemberId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<WorkSession>()
                .HasOne(x => x.ProjectMember).WithMany(x => x.WorkSessions).HasForeignKey(x => x.ProjectMemberId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(x => x.ProjectMembers).WithOne(x => x.User).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);


            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = 1,
                        FirstName = "Denys",
                        LastName = "Pavskyi",
                        UserName = "denys_pavskyi2",
                        Password = "password1",
                        Email = "den.pavski@gmai.com",
                        JiraId = "-"
                    }
                    
            );
        }

    }
}
