using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly KoiFarmDBContext _context;
        private DbSet<T> _dbSet;

        public KoiFarmDBContext(KoiFarmDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        protected DbSet<T> DbSet
        {
            get
            {
                if (_dbSet != null)
                {
                    return _dbSet;
                }

                _dbSet = _context.Set<T>();
                return _dbSet;
            }
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = DbSet.AsTracking();
            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<T, object>>[] array = includeProperties;
                foreach (Expression<Func<T, object>> navigationPropertyPath in array)
                {
                    queryable = queryable.Include(navigationPropertyPath);
                }
            }

            return predicate == null ? queryable : queryable.Where(predicate);
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(string id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
