using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DNP.Backbone.Comunes.Utilidades.ExtensionMethods
{
    public static class ReflectionMethods
    {
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }

        public static List<string> GetPropertyInfo<T>()
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            return propertyInfos.Select(propertyInfo => propertyInfo.Name).ToList();
        }

        public static List<string> GetPropertyInfo(object T)
        {
            var propertyInfos = T.GetType().GetProperties();
            return propertyInfos.Select(propertyInfo => propertyInfo.Name).ToList();
        }
    }
}
