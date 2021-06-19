using DailyNewsServer.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
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
            return _dBSet.AsNoTracking().SingleOrDefault(e => e.Id == id);
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

        public void Update(TEntity entity, bool saveChanges = true)
        {
            _dBSet.Update(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void UpdateRange(List<TEntity> entities, bool saveChanges = true)
        {
            _dBSet.UpdateRange(entities);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void Delete(int id, bool saveChanges = true)
        {
            var entity = FindByKey(id);
            if (entity == null)
            {
                throw new ArgumentNullException("delete entity cannot be found");
            }
            _dBSet.Remove(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void Insert(TEntity entity, bool saveChanges = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dBSet.Add(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
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
            return await _dBSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
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

        public async Task UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            _dBSet.Update(entity);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRangeAsync(List<TEntity> entities, bool saveChanges = true)
        {
            _dBSet.UpdateRange(entities);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, bool saveChanges = true)
        {
            var entity = await FindByKeyAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException("delete entity cannot be found");
            }
            _dBSet.Remove(entity);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task InsertAsync(TEntity entity, bool saveChanges = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dBSet.Add(entity);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
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
