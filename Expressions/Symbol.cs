using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Symbol : Expression
    {
        public string _name { get; set; }
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
                if (exp.IsPositive)
                {
                    Product p1 = new Product();
                    p1.Items.Add(new Number(2));
                    p1.Items.Add(this);
                    return p1;
                }
                else 
                    return new Number(0);
            }
            else if (exp.IsSum() || exp.IsProduct())
                return exp + this;
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
            if (exp.IsNumber() && ((Number)exp).GetValue().Abs() == 1)
                return this;
            else if (exp.IsPow() || exp.IsRational())
                return exp * this;
            else if (IsEqual(exp))
                return Extensions.CreatePow(this, new Number(2));
            else if (exp.IsNumber() || exp.IsSymbol())
                return Extensions.CreateProduct(this, exp);

            return exp * this;
        }
        protected override Expression Divide(Expression exp)
        {
            if (exp.IsSymbol() && this == exp)
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
            if (this == exp)
                return new Number(1);
            else
            {
                exp.IsPositive = false;
                Sum s1 = new Sum();
                s1.Items.Add(this);
                s1.Items.Add(exp);
                return s1;
            }
        }
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsSymbol())
                return _name == ((Symbol)exp)._name;
            else return false;
        }
    }
}
