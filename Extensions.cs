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
    }
}
