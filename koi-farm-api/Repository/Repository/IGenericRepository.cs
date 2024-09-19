using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public List<T> GetAll();
        public T GetById(string id);
        public void Create(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
    }
}
