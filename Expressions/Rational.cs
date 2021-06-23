using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Rational : Expression
    {
        public Expression _rationExp { get; set; }
        public Expression _denominatorExp { get; set; }

        public Rational(Expression RationExp, Expression DenominatorExp)
        {
            _rationExp = RationExp;
            _denominatorExp = DenominatorExp;
        }

        public override string Render()
        {
            return string.Format("{0}{1}/{2}", (IsPositive ? "" : "-"),_rationExp.Render(), _denominatorExp.Render());
        }
        public override Expression Simplify()
        {
            //TODO: normalde sadeleştirilebiliyorsa bu işlemi yapması lazım
            _rationExp = _rationExp.Simplify();
            _denominatorExp = _denominatorExp.Simplify();
            if (this._rationExp.IsNumber() && this._denominatorExp.IsNumber())
                return new Number(((Number)_rationExp).GetValue() / ((Number)_denominatorExp).GetValue());
            else if(_rationExp.IsProduct())
            {
                if(!_denominatorExp.IsProduct()) _denominatorExp = _denominatorExp.AsProduct();
                var common = _rationExp.FindCommon(_denominatorExp);
                if(common != null)
                {
                    _rationExp = _rationExp.ExcludeItem(common) ?? new Number(1);
                    _denominatorExp = _denominatorExp.ExcludeItem(common) ?? new Number(1);
                }
            }
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
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsSymbol())
                return Render() == exp.Render();
            else return false;
        }
    }
}
