using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;

namespace RestaurantApp.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _context { get; set; }
        private DbSet<T> _dbSet { get; set; }

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
                query = query.OrderBy(options.OrderBy);

            foreach (var include in options.GetIncludes())
                query = query.Include(include);

            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string primaryKeyName = key?.Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllByIdAsync<TKey>(TKey id, string propertyName, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
                query = query.OrderBy(options.OrderBy);

            foreach (var include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            query = query.Where(e => EF.Property<TKey>(e, propertyName).Equals(id));
            return await query.ToListAsync();
        }
    }
}