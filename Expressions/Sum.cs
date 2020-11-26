using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Sum : Expression
    {
        public bool IsPositive { get; set; }
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
            if (agg.IsNumber())
                return agg;
            else if(agg.IsSum())
                this.Items = agg.Items;

            return this;
        }
        protected override Expression Add(Expression exp)
        {
            if(exp.IsSum() || exp.IsProduct())
                Items.AddRange(exp.Items);
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
            return string.Format("{0}({1})", (IsPositive ? "" : "-"), string.Join("+", Items.Select(n => n.Render())));
        }
    }
}
