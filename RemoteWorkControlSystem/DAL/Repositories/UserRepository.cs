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
    public class UserRepository: IUserRepository
    {
        private readonly RemoteWorkControlSystemDbContext _context;

        public UserRepository(RemoteWorkControlSystemDbContext remoteWorkControlSystemDbContext)
        {
            _context = remoteWorkControlSystemDbContext;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var User = await _context.Users.FindAsync(id);
            _context.Users.Remove(User);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }



        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);

        }

        public async Task<User> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Users
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.WorkSessions)
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.EmployeeScreenActivities)
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await _context.Users
                .Include(x => x.ProjectMembers)
                    .ThenInclude(x => x.WorkSessions)
                .Include(x => x.ProjectMembers)
                .ThenInclude(x => x.EmployeeScreenActivities)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllWithNoTrackingAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
    }
}
