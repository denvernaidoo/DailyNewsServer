using DailyNewsServer.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Data.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext _context;
        internal DbSet<TEntity> _dBSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dBSet = _context.Set<TEntity>();
        }

        #region Synchronous Repository Methods
        public IEnumerable<TEntity> All()
        {
            return _dBSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> results = _dBSet.AsNoTracking()
                .Where(predicate).ToList();
            return results;
        }

        public TEntity FindByKey(int id)
        {
            var lambda = Utilities.BuildExpressionForFindByKey<TEntity>(id);
            return _dBSet.AsNoTracking().SingleOrDefault(lambda);
        }

        public IEnumerable<TEntity> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            return query.ToList();
        }

        public IEnumerable<TEntity> FindByInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = query.Where(predicate).ToList();
            return results;
        }

        public void Update(TEntity entity)
        {
            _dBSet.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _dBSet.UpdateRange(entities);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = FindByKey(id);
            if (entity == null)
            {
                throw new ArgumentNullException("delete entity cannot be found");
            }
            _dBSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dBSet.Add(entity);
            _context.SaveChanges();
        }
        #endregion

        #region Asynchronous Repository Methods
        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await _dBSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> results = await _dBSet.AsNoTracking()
                .Where(predicate).ToListAsync();
            return results;
        }

        public async Task<TEntity> FindByKeyAsync(int id)
        {
            var lambda = Utilities.BuildExpressionForFindByKey<TEntity>(id);
            return await _dBSet.AsNoTracking().SingleOrDefaultAsync(lambda);
        }

        public async Task<IEnumerable<TEntity>> AllIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindByIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = await query.Where(predicate).ToListAsync();
            return results;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dBSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<TEntity> entities)
        {
            _dBSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindByKeyAsync(id);
            _dBSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dBSet.Add(entity);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Common
        private IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dBSet.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperties) => current.Include(includeProperties));
        }
        #endregion
    }
}
