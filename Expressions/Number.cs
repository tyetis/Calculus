using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Number : Expression
    {
        private double _value { get; set; }
        public Number(double Value)
        {
            _value = Value;
            IsPositive = Value > 0;
        }
        public double GetValue()
        {
            return _value;
        }

        public override string Render()
        {
            return _value.ToString("0.#####");
        }
        public override Expression Simplify()
        {
            return this;
        }
        protected override Expression Add(Expression exp)
        {
            if (exp.IsNumber())
                return new Number(_value + ((Number)exp).GetValue());
            else
            {
                return exp + this;
            }
        }
        protected override Expression Multiply(Expression exp)
        {
            if (exp.IsNumber())
                return new Number(_value * ((Number)exp).GetValue());
            else
            {
                return exp * this;
            }
        }
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsNumber())
                return _value == ((Number)exp).GetValue();
            else return false;
        }
    }
}
