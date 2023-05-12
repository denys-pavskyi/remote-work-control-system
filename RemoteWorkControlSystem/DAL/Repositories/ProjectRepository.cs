using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProjectRepository: IProjectRepository
    {
        private readonly RemoteWorkControlSystemDbContext _context;

        public ProjectRepository(RemoteWorkControlSystemDbContext remoteWorkControlSystemDbContext)
        {
            _context = remoteWorkControlSystemDbContext;
        }

        public async Task AddAsync(Project entity)
        {
            await _context.Projects.AddAsync(entity);
        }

        public void Delete(Project entity)
        {
            _context.Projects.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var Project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(Project);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }



        public async Task<Project> GetByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public void Update(Project entity)
        {
            _context.Projects.Update(entity);

        }

        public async Task<Project> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Projects
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.EmployeeScreenActivities)
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.WorkSessions)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<IEnumerable<Project>> GetAllWithDetailsAsync()
        {
            return await _context.Projects
                .Include(x => x.ProjectMembers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllWithNoTrackingAsync()
        {
            return await _context.Projects.AsNoTracking().ToListAsync();
        }
    }
}
