using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Core.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        IEnumerable<TEntity> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> AllIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindByInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> FindByIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity FindByKey(int id);
        Task<TEntity> FindByKeyAsync(int id);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        Task UpdateRangeAsync(List<TEntity> entities);
        void Delete(int id);
        Task DeleteAsync(int id);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
    }
}
