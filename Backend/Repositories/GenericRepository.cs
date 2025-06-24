using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected readonly AppDbContext _db;
        public GenericRepository(AppDbContext db) => _db = db;

        public IQueryable<T> Query() =>
            _db.Set<T>().AsNoTracking();

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await Query().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _db.Set<T>().FindAsync(id);

        public async Task AddAsync(T entity) =>
            await _db.Set<T>().AddAsync(entity);

        public void Update(T entity) =>
            _db.Set<T>().Update(entity);

        public void Remove(T entity) =>
            _db.Set<T>().Remove(entity);

        public async Task SaveChangesAsync() =>
            await _db.SaveChangesAsync();
    }
}