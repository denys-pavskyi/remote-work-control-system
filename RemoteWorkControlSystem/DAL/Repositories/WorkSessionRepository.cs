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
    public class WorkSessionRepository: IWorkSessionRepository
    {
        private readonly RemoteWorkControlSystemDbContext _context;

        public WorkSessionRepository(RemoteWorkControlSystemDbContext remoteWorkControlSystemDbContext)
        {
            _context = remoteWorkControlSystemDbContext;
        }

        public async Task AddAsync(WorkSession entity)
        {
            await _context.WorkSessions.AddAsync(entity);
        }

        public void Delete(WorkSession entity)
        {
            _context.WorkSessions.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var WorkSession = await _context.WorkSessions.FindAsync(id);
            _context.WorkSessions.Remove(WorkSession);
        }

        public async Task<IEnumerable<WorkSession>> GetAllAsync()
        {
            return await _context.WorkSessions.ToListAsync();
        }



        public async Task<WorkSession> GetByIdAsync(int id)
        {
            return await _context.WorkSessions.FindAsync(id);
        }

        public void Update(WorkSession entity)
        {
            _context.WorkSessions.Update(entity);

        }

        public async Task<WorkSession> GetByIdWithDetailsAsync(int id)
        {
            return await _context.WorkSessions
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<IEnumerable<WorkSession>> GetAllWithDetailsAsync()
        {
            return await _context.WorkSessions
                 .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectMember)
                    .ThenInclude(x => x.Project)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkSession>> GetAllWithNoTrackingAsync()
        {
            return await _context.WorkSessions.AsNoTracking().ToListAsync();
        }
    }
}
