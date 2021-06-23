using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Sum : Expression
    {
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

            Expression agg = Items.Aggregate((i, e) => i + e);
            if (agg.IsNumber() || agg.IsProduct())
                return agg;
            else if (agg.IsSum())
                this.Items = agg.Items;

            return this;
        }
        protected override Expression Add(Expression exp)
        {
            if(exp.IsSum())
            {
                Items.AddRange(exp.Items);
                return this.Simplify();
            }
            else if (exp.IsNumber() && Extensions.HasNumber(this, out int index)) // Product bir sayı ile çarpılıyorsa
                Items[index] = Items[index] + exp;
            else if (exp.IsSymbol())
            {
                for(int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].IsProduct() 
                        && Items[i].ContainItem(exp)
                        || Items[i] == exp)
                    { Items[i] = Items[i] + exp; return this; }
                }
                Items.Add(exp);
            }
            else
                Items.Add(exp);
            return this;
        }
        protected override Expression Multiply(Expression exp)
        {
            Product s1 = new Product();
            s1.Items.Add(this);
            s1.Items.Add(exp);
            return s1;
        }
        public override string Render()
        {
            var str = string.Join("", Items.Select((n, i) => (n.IsPositive && i > 0 ? "+" : "") + n.Render()));
            return string.Format("{0}({1})", (IsPositive ? "" : "-"), str);
        }
        public override bool IsEqual(Expression exp)
        {
            if(exp.IsSum())
            {
                return Items.All(n => exp.Items.Any(e => e == n)) && Items.Count == exp.Items.Count;
            }
            return false;
        }
    }
}
