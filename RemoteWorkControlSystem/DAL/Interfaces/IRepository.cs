using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteByIdAsync(int id);

        void Update(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllWithNoTrackingAsync();


        Task<IEnumerable<TEntity>> GetAllWithDetailsAsync();
        Task<TEntity> GetByIdWithDetailsAsync(int id);

    }
}
