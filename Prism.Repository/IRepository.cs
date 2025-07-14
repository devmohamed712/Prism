using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Prism.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAll_(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetByPage(int pageNumber, int pageSize);
        IEnumerable<TEntity> FindList(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> FindListOrderByDesc(Func<TEntity, bool> predicate, Func<TEntity, int> orderPredicate);
        IEnumerable<TEntity> FindByPage(Func<TEntity, bool> predicate, int pageNumber, int pageSize);
        IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal?> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, DateTime> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, DateTime?> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByAsc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, int> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, decimal?> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageSize, Func<TEntity, int> orderPredicate, int skip);
        IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity,bool> predicate,int pageNumber, int pageSize, Func<TEntity,int> orderPredicate);
        IEnumerable<TEntity> GetByPageOrderByDesc(Func<TEntity, bool> predicate, int pageNumber, int pageSize, Func<TEntity, DateTime> orderPredicate);
        TEntity FirstOrDefault(Func<TEntity, bool> predicate);
        TEntity LastOrDefault(Func<TEntity, bool> predicate);
        int Count(Func<TEntity, bool> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        bool Any(Func<TEntity, bool> predicate);

    }
}
