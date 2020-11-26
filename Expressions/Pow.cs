using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Pow : Expression
    {
        public Expression _powExp { get; set; }
        public Expression _baseExp { get; set; }

        public Pow(Expression BaseExp, Expression PowExp)
        {
            _baseExp = BaseExp;
            _powExp = PowExp;
        }

        public override string Render()
        {
            return string.Format("{0}^{1}", _baseExp.Render(), _powExp.Render());
        }
        public override Expression Simplify()
        {
            //TODO: normalde sadeleştirilebiliyorsa bu işlemi yapması lazım
            _baseExp = _baseExp.Simplify();
            _powExp = _powExp.Simplify();
            if (this._baseExp.IsNumber() && this._powExp.IsNumber())
                return new Number((double)Math.Pow(((Number)_baseExp).GetValue(), ((Number)_powExp).GetValue()));
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
            Product p1 = new Product();
            p1.Items.Add(this);
            p1.Items.Add(exp);
            return p1;
        }
    }
}
