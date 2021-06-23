using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Calculus
{
    public class Parser
    {
        public static Expression Parse(string Expression, List<string> parantesis = null)
        {
            bool IsPositiveFlag = !Expression.StartsWith("-");
            if (parantesis == null) parantesis = new List<string>();

            if (Regex.IsMatch(Expression, @"^[+-]?\{(\d+)\}$"))
            {
                if (Expression.StartsWith("-")) IsPositiveFlag = false;
                Match m = Regex.Match(Expression, @"^[+-]?\{(\d+)\}$");
                if (m != null)
                {
                    Expression = parantesis[Convert.ToInt32(m.Groups[1].Value)];
                }
            }

            if (Extensions.IsNumber(Expression))
            {
                return new Number(Convert.ToDouble(Expression));
            }

            MatchCollection mc_symbol = Regex.Matches(Expression, @"^[+-]?([a-zA-Z])$");
            if (mc_symbol.Count > 0)
            {
                Symbol sym = new Symbol(mc_symbol[0].Groups[1].Value, !mc_symbol[0].Value.StartsWith("-"));
                sym.IsPositive = IsPositiveFlag;
                return sym;
            }

            MatchCollection mc_parantesis = Regex.Matches(Expression, @"\(((?>[^()]+|\((?<n>)|\)(?<-n>))+(?(n)(?!)))\)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            int _oldcount = parantesis.Count;
            for (int i = 0; i < mc_parantesis.Count; i++)
            {
                parantesis.Add(mc_parantesis[i].Groups[1].Value);
                Expression = Expression.Replace(mc_parantesis[i].Value, "{" + (i + _oldcount) + "}");
            }

            MatchCollection mc_sum = Regex.Matches(Expression, @"[+-]?([\w,^\/*{}]+)");
            if (mc_sum.Count > 1)
            {
                Sum sum = new Sum();
                sum.IsPositive = IsPositiveFlag;
                for (int i = 0; i < mc_sum.Count; i++)
                {
                    sum.Items.Add(Parse(mc_sum[i].Value, parantesis));
                }
                return sum;
            }

            MatchCollection mc_product = Regex.Matches(Expression, @"[\*]?([\w,^\/+-{}]+)");
            if (mc_product.Count > 1)
            {
                Product p = new Product();
                p.IsPositive = IsPositiveFlag;
                for (int i = 0; i < mc_product.Count; i++)
                {
                    p.Items.Add(Parse(mc_product[i].Groups[1].Value, parantesis));
                }
                return p;
            }

            Match m_rational = Regex.Match(Expression, @"([\w,{}^]+)\/([\w,{}^]+)");
            if (m_rational.Groups.Count > 1)
            {
                Rational r = new Rational(Parse(m_rational.Groups[1].Value, parantesis), Parse(m_rational.Groups[2].Value, parantesis));
                r.IsPositive = IsPositiveFlag;
                return r;
            }

            Match m_pow = Regex.Match(Expression, @"([\w,{}]+)\^([\w,{}]+)");
            if (m_pow.Groups.Count > 1)
            {
                Pow p = new Pow(Parse(m_pow.Groups[1].Value, parantesis), Parse(m_pow.Groups[2].Value, parantesis));
                p.IsPositive = IsPositiveFlag;
                return p;
            }

            return Parse(Expression, parantesis); // hiç bir eşleşme yoksa tekrar parçalamayı dene
        }
    }
}
