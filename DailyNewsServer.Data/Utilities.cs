using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DailyNewsServer.Data
{
    public static class Utilities
    {
        //sample to build lambda to select a key where the key name is made up using the entity name (like UserId/AccountId/InvoiceId in this implementation), this is better than DbSet.Find() only when you want no tracking
        public static Expression<Func<TEntity, bool>> BuildExpressionForFindByKey<TEntity>(int id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, typeof(TEntity).Name + "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);

            return lambda;
        }
    }
}
