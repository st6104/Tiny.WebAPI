// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Tiny.Infrastructure.Abstract.Utilities;

public static class EFexpression
{
    private static MethodCallExpression? GetPropertyCallExpression<TProperty>(Expression entityParameter,
        string fieldName)
    {
        var methodInfo = typeof(EF).GetMethod(nameof(EF.Property))?.MakeGenericMethod(typeof(TProperty));
        if (methodInfo == null)
        {
            return null;
        }

        var propertyNameConstant = Expression.Constant(fieldName);

        return Expression.Call(methodInfo, entityParameter, propertyNameConstant);
    }

    public static BinaryExpression? GetPropertyValueEqualityExpression<TProperty>(ParameterExpression entityParameter,
        string fieldName, TProperty value)
    {
        var propertyCallExpression = GetPropertyCallExpression<TProperty>(entityParameter, fieldName);
        var valueExpression = Expression.Constant(value, typeof(TProperty));

        return propertyCallExpression == null ? null : Expression.Equal(propertyCallExpression, valueExpression);
    }
}
