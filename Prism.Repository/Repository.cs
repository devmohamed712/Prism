using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Prism.DAL;
using System.Drawing;

namespace Prism.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        public Repository(DbContext context)
        {
            Context = context;
        }
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public int Count(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Count();
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public IEnumerable<TEntity> GetByPage(int pageNumber, int pageSize)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Skip(count).Take(pageSize).ToList();
        }

        public IEnumerable<TEntity> FindList(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public IEnumerable<TEntity> FindListOrderByDesc(Func<TEntity, bool> predicate, Func<TEntity, int> orderPredicate)
        {
            return Context.Set<TEntity>().Where(predicate).OrderBy(orderPredicate).ToList();
        }

        public IEnumerable<TEntity> FindByPage(Func<TEntity, bool> predicate, int pageNumber, int pageSize)
        {
            int skip = (pageNumber * pageSize) - pageSize;
            return Context.Set<TEntity>().Where(predicate).Skip(skip).Take(pageSize).ToList();
        }
      
        public IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderBy(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal?> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderBy(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, DateTime> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderBy(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, DateTime?> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderBy(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, int> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderBy(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageSize, Func<TEntity, int> orderPredicate ,int skip)
        {
            return Context.Set<TEntity>().Where(predicate).OrderByDescending(orderPredicate).Skip(skip).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderByDescending(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal?> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderByDescending(orderPredicate).Skip(count).Take(pageSize).ToList();
        }
        public IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, int> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderByDescending(orderPredicate).Skip(count).Take(pageSize).ToList();
        }

        public IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, DateTime> orderPredicate)
        {
            int count = (pageSize * pageNumber) - pageSize;
            return Context.Set<TEntity>().Where(predicate).OrderByDescending(orderPredicate).Skip(count).Take(pageSize).ToList();
        }

        public TEntity LastOrDefault(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().LastOrDefault(predicate);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public bool Any(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Any();
        }
        public IEnumerable<TEntity> GetAll_(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> data = Context.Set<TEntity>().Where(predicate);
            return data.ToList();
        }
    }
}
