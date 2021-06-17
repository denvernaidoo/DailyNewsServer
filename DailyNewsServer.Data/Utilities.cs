using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DailyNewsServer.Data
{
    public static class Utilities
    {
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
