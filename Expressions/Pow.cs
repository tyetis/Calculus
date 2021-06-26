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
            return string.Format("{0}{1}^{2}", (IsPositive ? "" : "-"), _baseExp.RenderWrapParantesis(), _powExp.RenderWrapParantesis());
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
            if(_baseExp == Extensions.FindBase(exp))
            {
                if (exp.IsPow())
                    _powExp += ((Pow)exp)._powExp;
                else
                    _powExp += new Number(1);
                if (_powExp == new Number(1))
                    return _baseExp;
                return this;
            }
            return Extensions.CreateProduct(this, exp);
        }
        protected override Expression Divide(Expression exp)
        {
            if (_baseExp == Extensions.FindBase(exp))
            {
                if (exp.IsPow())
                    _powExp -= ((Pow)exp)._powExp;
                else
                    _powExp -= new Number(1);
                if (_powExp == new Number(1))
                    return _baseExp;
                return this;
            }
            return Extensions.CreateRational(this, exp);
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
    }
}
