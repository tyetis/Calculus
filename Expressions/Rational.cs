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
            return string.Format("{0}{1}/{2}", (IsPositive ? "" : "-"),_rationExp.RenderWrapParantesis(), _denominatorExp.RenderWrapParantesis());
        }
        public override Expression Simplify()
        {
            //TODO: normalde sadeleştirilebiliyorsa bu işlemi yapması lazım
            _rationExp = _rationExp.Simplify();
            _denominatorExp = _denominatorExp.Simplify();

            return _rationExp / _denominatorExp;
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
            if (exp.IsPow() || exp.IsSymbol())
            {
                _rationExp = exp * _rationExp;
                return this;
            }
            if (exp.IsRational())
            {
                _rationExp *= ((Rational)exp)._rationExp;
                _denominatorExp *= ((Rational)exp)._denominatorExp;
                return this;
            }
            return this;
        }
        protected override Expression Divide(Expression exp)
        {
            if (exp.IsPow() || exp.IsSymbol())
            {
                _rationExp = exp / _rationExp;
                return this;
            }
            if (exp.IsRational())
            {
                _rationExp /= ((Rational)exp)._rationExp;
                _denominatorExp /= ((Rational)exp)._denominatorExp;
                return this;
            }
            return this;
        }
        protected override Expression Extract(Expression exp)
        {
            return this;
        }
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsSymbol())
                return Render() == exp.Render();
            else return false;
        }
        public Expression ReverseDivision()
        {
            var _ = _rationExp;
            _rationExp = _denominatorExp;
            _denominatorExp = _;
            return this;
        }
    }
}
