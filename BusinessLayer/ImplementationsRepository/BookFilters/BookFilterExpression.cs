using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using Library.Tag;

namespace Library.Services.BookContorlServices.BookFilters
{
    public static class BookFilterExpression
    {
        public static IQueryable<TElement> WhereComplex<TElement, TKey>(this IQueryable<TElement> set, TKey element)
        {
            PropertyInfo[] props = element.GetType().GetProperties();

            foreach (var prop in props)
            {
                var propValue = prop?.GetValue(element)?.ToString();
                if (prop.PropertyType == typeof(string) && !(String.IsNullOrWhiteSpace(propValue)))
                {
                    var setParametrExpression = Expression.Parameter(typeof(TElement), "prop");
                    var setProperty = Expression.Property(setParametrExpression, prop.Name);
                    var modelConstant = Expression.Constant(propValue, typeof(string));
                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var callMethod = Expression.Call(setProperty, method, modelConstant);
                    var lambda = Expression.Lambda<Func<TElement, bool>>(callMethod, setParametrExpression);
                    set = set.Where(lambda);
                }
            }

            return set;
        }

        public static IQueryable<TElement> OrderByComplex<TElement>(this IQueryable<TElement> set, SortState element)
        {
            var sortState = element.ToString();
            var isAsc = sortState.EndsWith("Asc");
            var property = sortState.Remove(sortState.Length - 3);

            var setParametrExpression = Expression.Parameter(typeof(TElement), "prop");
            var returnProperty = Expression.Property(setParametrExpression, property);
            var lambda = Expression.Lambda<Func<TElement, string>>(returnProperty, setParametrExpression);
            if (isAsc)
                set = set.OrderBy(lambda);
            else
                set = set.OrderByDescending(lambda);

            return set;
        }
    }
}
