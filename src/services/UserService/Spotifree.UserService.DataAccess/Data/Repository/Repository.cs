using Microsoft.EntityFrameworkCore;
using Spotifree.UserService.DataAccess.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spotifree.UserService.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            this.Context = context;
            this.DbSet = Context.Set<T>();
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            // include properties will be comma separated
            if (includeProperties == null) return orderBy != null ? orderBy(query).ToList() : query.ToList();
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            // include properties will be comma separated
            if (includeProperties == null) return query.FirstOrDefault();
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.FirstOrDefault();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(int id)
        {
            T entityToRemove = DbSet.Find(id);
            Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
