using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProjectMemberRepository: IProjectMemberRepository
    {

        private readonly RemoteWorkControlSystemDbContext _context;


        public ProjectMemberRepository(RemoteWorkControlSystemDbContext remoteWorkControlSystemDbContext)
        {
            _context = remoteWorkControlSystemDbContext;
        }

        public async Task AddAsync(ProjectMember entity)
        {
            await _context.ProjectMembers.AddAsync(entity);
        }

        public void Delete(ProjectMember entity)
        {
            _context.ProjectMembers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var ProjectMember = await _context.ProjectMembers.FindAsync(id);
            _context.ProjectMembers.Remove(ProjectMember);
        }

        public async Task<IEnumerable<ProjectMember>> GetAllAsync()
        {
            return await _context.ProjectMembers.ToListAsync();
        }



        public async Task<ProjectMember> GetByIdAsync(int id)
        {
            return await _context.ProjectMembers.FindAsync(id);
        }

        public void Update(ProjectMember entity)
        {
            _context.ProjectMembers.Update(entity);

        }

        public async Task<ProjectMember> GetByIdWithDetailsAsync(int id)
        {
            return await _context.ProjectMembers
                .Include(x => x.User)
                .Include(x => x.EmployeeScreenActivities)
                .Include(x => x.TaskDurations)
                .Include(x => x.WorkSessions)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<IEnumerable<ProjectMember>> GetAllWithDetailsAsync()
        {
            return await _context.ProjectMembers
                .Include(x => x.User)
                .Include(x => x.EmployeeScreenActivities)
                .Include(x => x.TaskDurations)
                .Include(x => x.WorkSessions)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetAllWithNoTrackingAsync()
        {
            return await _context.ProjectMembers.AsNoTracking().ToListAsync();
        }

    }
}
