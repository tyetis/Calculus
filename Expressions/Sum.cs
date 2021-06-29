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
            for (int i = 0; i < Items.Count; i++)
                Items[i] = Items[i].Simplify();
            return Items.Aggregate((i, e) => i + e).Order();
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
        protected override Expression Divide(Expression exp)
        {
            if (exp.IsSum() && this == exp)
                return new Number(1);
            else if (exp.IsProduct())
            {
                var reverseDiv = exp / this;
                if (reverseDiv.IsRational())
                    return ((Rational)reverseDiv).ReverseDivision();
                else return new Number(1) / reverseDiv;
            }
            return Extensions.CreateRational(this, exp);
        }
        protected override Expression Extract(Expression exp)
        {
            return this;
        }
        public override string Render()
        {
            var str = string.Join("", Items.Select((n, i) => (n.IsPositive && i > 0 ? "+" : "") + n.Render()));
            return !IsPositive ? string.Format("-({0})", str) : str;
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
