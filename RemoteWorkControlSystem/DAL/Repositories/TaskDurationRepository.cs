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
    public class TaskDurationRepository: ITaskDurationRepository
    {
        private readonly RemoteWorkControlSystemDbContext _context;

        public TaskDurationRepository(RemoteWorkControlSystemDbContext remoteWorkControlSystemDbContext)
        {
            _context = remoteWorkControlSystemDbContext;
        }

        public async Task AddAsync(TaskDuration entity)
        {
            await _context.TaskDurations.AddAsync(entity);
        }

        public void Delete(TaskDuration entity)
        {
            _context.TaskDurations.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var TaskDuration = await _context.TaskDurations.FindAsync(id);
            _context.TaskDurations.Remove(TaskDuration);
        }

        public async Task<IEnumerable<TaskDuration>> GetAllAsync()
        {
            return await _context.TaskDurations.ToListAsync();
        }



        public async Task<TaskDuration> GetByIdAsync(int id)
        {
            return await _context.TaskDurations.FindAsync(id);
        }

        public void Update(TaskDuration entity)
        {
            _context.TaskDurations.Update(entity);

        }

        public async Task<TaskDuration> GetByIdWithDetailsAsync(int id)
        {
            return await _context.TaskDurations
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.WorkSessions)
                .Include(x => x.ProjectMember)
                .ThenInclude(x => x.EmployeeScreenActivities)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<IEnumerable<TaskDuration>> GetAllWithDetailsAsync()
        {
            return await _context.TaskDurations
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.WorkSessions)
                .Include(x => x.ProjectMember)
                .ThenInclude(x => x.EmployeeScreenActivities)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskDuration>> GetAllWithNoTrackingAsync()
        {
            return await _context.TaskDurations.AsNoTracking().ToListAsync();
        }

    }
}
