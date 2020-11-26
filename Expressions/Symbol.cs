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
        private bool _isPositive { get; set; }
        public Symbol(string Name, bool IsPositive = true)
        {
            _name = Name;
            _isPositive = IsPositive;
        }

        public override string Render()
        {
            return _name;
        }
        public override Expression Simplify()
        {
            return this;
        }
        protected override Expression Add(Expression exp)
        {
            Sum s1 = new Sum();
            s1.Items.Add(this);
            s1.Items.Add(exp);
            return s1;
        }
        protected override Expression Multiply(Expression exp)
        {
            Product s1 = new Product();
            s1.Items.Add(this);
            s1.Items.Add(exp);
            return s1;
        }
    }
}
