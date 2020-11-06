using System.Linq.Expressions;

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

        public static string GetPropertyName(this MemberExpression expression) => expression.Member.Name;

        public static string GetPropertyName(this LambdaExpression expression) => expression.GetMemberExp().GetPropertyName();

    }
}

