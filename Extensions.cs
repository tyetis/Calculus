using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Calculus
{
    public static class Extensions
    {
        public static int dept = 0;
        public static string Space(int count)
        {
            string val = "";
            for (int i = 0; i < count; i++)
                val += " ";
            return val;
        }
        public static bool HasNumber(Expression expression, out int index)
        {
            index = expression.Items.FindIndex(n => n.IsNumber());
            return index >= 0;
        }

        public static bool IsSingleItem(this Expression expression)
        {
            return expression.Items.Count == 1;
        }
        public static bool IsNumber(this Expression expression)
        {
            return expression.GetType() == typeof(Number);
        }
        public static bool IsSymbol(this Expression expression)
        {
            return expression.GetType() == typeof(Symbol);
        }
        public static bool IsPow(this Expression expression)
        {
            return expression.GetType() == typeof(Pow);
        }
        public static bool IsRational(this Expression expression)
        {
            return expression.GetType() == typeof(Rational);
        }
        public static bool IsNumber(this string expression)
        {
            double a;
            return double.TryParse(expression, out a);
        }
        public static bool IsSymbol(this string expression)
        {
            return Regex.IsMatch(expression, "^[+-]?[a-zA-Z]$", RegexOptions.Singleline);
        }
        public static bool IsSum(this Expression expression)
        {
            return expression.GetType() == typeof(Sum);
        }
        public static bool IsProduct(this Expression expression)
        {
            return expression.GetType() == typeof(Product);
        }

        public static Expression ExcludeItem(this Expression expression, Expression excluded)
        {
            expression.Items = expression.Items.Where(n => n != excluded).ToList();
            if (expression.IsSingleItem())
                return expression.Items[0];
            else if (expression.Items.Count == 0)
                return null;
            else
                return expression;
        }
        public static bool ContainItem(this Expression expression, Expression exp)
        {
            return expression.Items.Any(n => n == exp);
        }
        public static Expression FindCommon(this Expression expression, Expression exp)
        {
            return expression.Items.FirstOrDefault(n => exp.Items.Any(e => n == e));
        }
        public static Product AsProduct(this Expression expression)
        {
            Product p1 = new Product();
            p1.Items.Add(expression);
            return p1;
        }

    }
}
