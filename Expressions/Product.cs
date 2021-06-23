using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Product : Expression
    {
        public override string Render()
        {
            return string.Format("{0}{1}", (IsPositive ? "" : "-") ,string.Join("*", Items.Select(n => n.Render())));
        }
        public override Expression Simplify()
        {
            foreach (Expression e in Items.ToList())
            {
                if (e.IsSum() || e.IsProduct())
                {
                    Expression simplified = e.Simplify();
                    if (e.IsSingleItem())
                    {
                        Items.Remove(e);
                        Items.Add(e.Items.FirstOrDefault());
                    }
                    else if (simplified.IsNumber())
                    {
                        Items.Remove(e);
                        Items.Add(simplified);
                    }
                }
                else if (e.IsPow() || e.IsRational()) //TODO: normalde sadeleştirilebiliyorsa bu işlemi yapması lazım
                {
                    Items.Remove(e);
                    Items.Add(e.Simplify());
                }
            }

            Expression agg = Items.Aggregate((i, e) => i * e);
            if (agg.IsNumber())
                return agg;
            else if (agg.IsProduct())
                this.Items = agg.Items;

            return this;
        }
        protected override Expression Add(Expression exp)
        {
            if (exp.IsProduct())
            {
                var common = this.FindCommon(exp);
                if(common != null)
                {
                    Sum s1 = new Sum();
                    s1.Items.Add(this.ExcludeItem(common));
                    s1.Items.Add(exp.ExcludeItem(common));
                    Product p1 = new Product();
                    p1.Items.Add(s1.Simplify());
                    p1.Items.Add(common);
                    return p1;
                }
                else return this;
            }
            else if (exp.IsSymbol() && Items.Any(n => n == exp))
            {
                if(Extensions.HasNumber(this, out int index))
                    Items[index] = Items[index] + new Number(1);
                else
                    Items.Add(new Number(2));
                return this;
            }
            else
            {
                Sum s1 = new Sum();
                s1.Items.Add(this);
                s1.Items.Add(exp);
                return s1;
            }
        }
        protected override Expression Multiply(Expression exp)
        {
            if (exp.IsSum() || exp.IsProduct())
                Items.AddRange(exp.Items);
            else if (exp.IsNumber() && Extensions.HasNumber(this, out int index)) // Product bir sayı ile çarpılıyorsa
                Items[index] = Items[index] * exp;
            else Items.Add(exp);
            return this;
        }
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsSymbol())
                return false;
            else return false;
        }
    }
}
