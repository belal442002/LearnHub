using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LearnHub.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LearnHubDbContext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(LearnHubDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }
        public async Task<bool> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            _dbSet.AddRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync<DT>(DT id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            //_dbSet.Attach(entity);
            //_dbContext.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
