using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace X.NavView.Helpers
{
    internal static class ExpressionHelper
    {
        public static MemberExpression GetMemberExp(this LambdaExpression expression)
        {
            if (expression.Body is UnaryExpression unaryExpr)
            {
                return unaryExpr.Operand as MemberExpression;
            }

            return expression.Body as MemberExpression;
        }

        public static string GetPropertyName(this MemberExpression expression)
        {
            return expression.Member.Name;
        }

        public static string GetPropertyName(this LambdaExpression expression)
        {
            return expression.GetMemberExp().GetPropertyName();
        }

    }
}

