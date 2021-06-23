using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Symbol : Expression
    {
        private string _name { get; set; }
        public Symbol(string Name, bool isPositive = true)
        {
            _name = Name;
            IsPositive = isPositive;
        }

        public override string Render()
        {
            return string.Format("{0}{1}", (IsPositive ? "" : "-"), _name);
        }
        public override Expression Simplify()
        {
            return this;
        }
        protected override Expression Add(Expression exp)
        {
            if (this == exp)
            {
                Product p1 = new Product();
                p1.Items.Add(new Number(2));
                p1.Items.Add(this);
                return p1;
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
            Product s1 = new Product();
            s1.Items.Add(this);
            s1.Items.Add(exp);
            return s1;
        }
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsSymbol())
                return Render() == exp.Render();
            else return false;
        }
    }
}
