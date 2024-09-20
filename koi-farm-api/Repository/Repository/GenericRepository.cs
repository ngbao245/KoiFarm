using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly KoiFarmDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(KoiFarmDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Retrieves all entities
        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        // Retrieves an entity by its string ID (convert if necessary)
        public T GetById(string id)
        {
            return _dbSet.Find(id);
        }

        // Creates a new entity
        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        // Updates an existing entity
        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Deletes an entity
        //public void Delete(T entity)
        //{
        //    if (_context.Entry(entity).State == EntityState.Detached)
        //    {
        //        _dbSet.Attach(entity);
        //    }
        //    _dbSet.Remove(entity);
        //    _context.SaveChanges();
        //}

        //Soft deletion
        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            // Perform the soft delete
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAt = DateTimeOffset.UtcNow;
                _dbSet.Update(entity);
            }

            _context.SaveChanges();
        }

        // Retrieves a single entity based on a predicate, with optional related entities
        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            // Include related entities
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.SingleOrDefault(predicate);
        }

        // Retrieves a collection of entities based on a predicate, with optional related entities
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            // Apply the predicate if it's provided
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Include related entities
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }
    }
}
