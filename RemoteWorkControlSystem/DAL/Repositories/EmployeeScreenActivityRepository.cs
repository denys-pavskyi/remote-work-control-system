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
    public class EmployeeScreenActivityRepository: IEmployeeScreenActivityRepository
    {
        private readonly RemoteWorkControlSystemDbContext _context;

        public EmployeeScreenActivityRepository(RemoteWorkControlSystemDbContext remoteWorkControlSystemDbContext)
        {
            _context = remoteWorkControlSystemDbContext;
        }

        public async Task AddAsync(EmployeeScreenActivity entity)
        {
            await _context.EmployeeScreenActivities.AddAsync(entity);
        }

        public void Delete(EmployeeScreenActivity entity)
        {
            _context.EmployeeScreenActivities.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var EmployeeScreenActivity = await _context.EmployeeScreenActivities.FindAsync(id);
            _context.EmployeeScreenActivities.Remove(EmployeeScreenActivity);
        }

        public async Task<IEnumerable<EmployeeScreenActivity>> GetAllAsync()
        {
            return await _context.EmployeeScreenActivities.ToListAsync();
        }



        public async Task<EmployeeScreenActivity> GetByIdAsync(int id)
        {
            return await _context.EmployeeScreenActivities.FindAsync(id);
        }

        public void Update(EmployeeScreenActivity entity)
        {
            _context.EmployeeScreenActivities.Update(entity);

        }

        public async Task<EmployeeScreenActivity> GetByIdWithDetailsAsync(int id)
        {
            return await _context.EmployeeScreenActivities
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.TaskDurations)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.WorkSessions)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<IEnumerable<EmployeeScreenActivity>> GetAllWithDetailsAsync()
        {
            return await _context.EmployeeScreenActivities
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.TaskDurations)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.WorkSessions)
                .Include(x => x.ProjectMember)
                .ThenInclude(x => x.EmployeeScreenActivities)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeScreenActivity>> GetAllWithNoTrackingAsync()
        {
            return await _context.EmployeeScreenActivities.AsNoTracking().ToListAsync();
        }


    }
}
