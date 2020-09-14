﻿using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;

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
    }
}