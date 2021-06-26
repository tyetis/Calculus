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
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }
        public static bool HasNumber(Expression expression, out int index)
        {
            bool result = FindItemIndex(expression, n => n.IsNumber(), out int _index);
            index = _index;
            return result;
        }
        public static bool FindItemIndex(this Expression expression, Predicate<Expression> match, out int index)
        {
            index = expression.Items.FindIndex(match);
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

        public static bool ContainItem(this Expression expression, Expression exp)
        {
            return expression.Items.Any(n => n == exp);
        }
        public static Product AsProduct(this Expression expression)
        {
            Product p1 = new Product();
            p1.Items.Add(expression);
            return p1;
        }
        public static Sum CreateSum(params Expression[] expressions)
        {
            Sum s1 = new Sum();
            foreach (var s in expressions)
                s1.Items.Add(s);
            return s1;
        }
        public static Product CreateProduct(params Expression[] expressions)
        {
            Product p1 = new Product();
            foreach (var s in expressions)
                p1.Items.Add(s);
            return p1;
        }
        public static Expression CreateRational(Expression ration, Expression denominator)
        {
            if (denominator.IsNumber() && ((Number)denominator).GetValue() == 1) return ration;
            Rational r1 = new Rational(ration, denominator);
            return r1;
        }
        public static Expression CreatePow(Expression _base, Expression pow)
        {
            Pow p1 = new Pow(_base, pow);
            return p1;
        }
        public static Expression FindBase(Expression expression)
        {
            if (expression.IsRational()) return ((Rational)expression)._rationExp;
            else if (expression.IsPow()) return ((Pow)expression)._baseExp;
            else return expression;
        }
        public static Expression IncreasePow(Expression expression, Expression pow)
        {
            if (expression.IsPow())
            {
                ((Pow)expression)._powExp += pow;
                return expression;
            }
            return CreatePow(expression, pow);
        }
        public static Expression ExcludeItem(this Expression expression, Expression excluded)
        {
            if (expression.IsProduct())
            {
                expression.Items = expression.Items.Where(n => n != excluded).ToList();
                if (expression.IsSingleItem())
                    return expression.Items[0];
                else if (expression.Items.Count == 0)
                    return null;
                return expression;
            }
            else if (expression.IsEqual(excluded))
                return new Number(1);
            else return expression;
        }
        public static string RenderWrapParantesis(this Expression expression)
        {
            if (!(expression.IsSymbol() || expression.IsNumber()))
                return string.Format("({0})", expression.Render());
            else return expression.Render();
        }
    }
}
