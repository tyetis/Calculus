using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Product : Expression
    {
        public override string Render()
        {
            string _ = string.Join("*", Items.Select(n => n.RenderWrapParantesis()));
            return IsPositive ? _ : string.Format("-({0})", _);
        }
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

            return Items.Aggregate((i, e) => i * e).Order();
        }
        protected override Expression Add(Expression exp)
        {
            if (exp.IsProduct() || exp.IsSymbol())
            {
                var common = FindCommonFactor(exp);
                if (common != null)
                    return (this.ExcludeItem(common) + exp.ExcludeItem(common)) * common;
            }
            else if ((exp.IsSymbol() || exp.IsSum()) && IsEqualWithoutCoefficient(exp))
                return CoefficientIncrease();
            return Extensions.CreateSum(this, exp);
        }
        protected override Expression Multiply(Expression exp)
        {
            if (exp.IsProduct())
            {
                Items.AddRange(exp.Items);
                return this.Simplify();
            }
            else if (exp.IsPow() || exp.IsSum() || exp.IsSymbol())
            {
                var _baseExp = Extensions.FindBase(exp);
                if(this.FindItemIndex(n => Extensions.FindBase(n).IsEqual(_baseExp), out int commonBase))
                {
                    Items[commonBase] *= exp;
                    return this;
                }
            }
            else if (exp.IsNumber()) // Product bir sayı ile çarpılıyorsa
            {
                Number nmbr = (Number)exp;
                if (Extensions.HasNumber(this, out int index))
                    Items[index] = Items[index] * exp;
                else if(nmbr.GetValue().Abs() != 1)
                    Items.Add(nmbr);
                return this;
            }
            Items.Add(exp);
            return this;
        }
        protected override Expression Divide(Expression exp)
        {
            Product division = (exp.IsProduct()) ? (Product)exp : exp.AsProduct();
            var common = FindCommonFactor(division);
            if (common != null)
                return (this.ExcludeItem(common) / (division.ExcludeItem(common) ?? new Number(1)));

            return Extensions.CreateRational(this, exp);
        }
        protected override Expression Extract(Expression exp)
        {
            return this;
        }
        public override bool IsEqual(Expression exp)
        {
            if (exp.IsProduct())
            {
                return Items.All(n => exp.Items.Any(e => e == n)) && Items.Count == exp.Items.Count;
            }
            else return false;
        }
        public bool IsEqualWithoutCoefficient(Expression ex)
        {
            return Items.Where(n => !n.IsNumber()).All(n => n == ex);
        }
        public Expression CoefficientIncrease()
        {
            if (Extensions.HasNumber(this, out int index))
                Items[index] = Items[index] + new Number(1);
            else
                Items.Add(new Number(2));
            return this;
        }
        public Expression FindCommonFactor(Expression exp)
        {
            if (exp.IsProduct())
                return Items.FirstOrDefault(n => exp.Items.Any(e => n == e));
            else if (exp.IsSymbol())
                return Items.FirstOrDefault(n => exp == n);
            else return null;
        }
    }
}
