using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using Quartz.Util;
using Library.Models;
using System.Collections.Generic;

namespace Library.Services.BookContorlServices.BookFilters
{
    public static class BookFilterExpression
    {
        public static IQueryable<TElement> WhereComplex<TElement, TKey>(this IEnumerable<TElement> set, TKey element)
        {
            IQueryable<TElement> outpuSets = set.AsQueryable<TElement>();
            PropertyInfo[] props = element.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.PropertyType.ToString() == "System.String" && !(String.IsNullOrWhiteSpace(prop?.GetValue(element)?.ToString())))
                {
                    Expression<Func<TElement, bool>> lamda
                        = q => q.GetType().GetProperty(prop.Name).GetValue(q).ToString().Contains(element.GetType().GetProperty(prop.Name).GetValue(element).ToString());

                    outpuSets = outpuSets.Where(lamda);
                }
            }
            return outpuSets;
        }
        public static IQueryable<TElement> WhereIfNotWhiteOrNull<TElement>(this IQueryable<TElement> set, Expression<Func<TElement, bool>> exp)
        {
            if (exp.Body is MethodCallExpression methodCall && methodCall.Object.Type.ToString() == "System.String")
            {
                Console.WriteLine(methodCall.Arguments[0].ToString());
                if (methodCall.Arguments[0].ToString().IsNullOrWhiteSpace())
                    return set;

                return set.Where(exp);
            }
            return set;

        }
    }
}
