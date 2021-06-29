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
            for (int i = 0; i < Items.Count; i++)
                Items[i] = Items[i].Simplify();
            return Items.Aggregate((i, e) => i * e).Order();
        }
        protected override Expression Add(Expression exp)
        {
            Product p = (Product)(exp.IsProduct() ? exp : exp.AsProduct());
            var first = SplitCoefficient();
            var second = p.SplitCoefficient();
            if (first.Item2 == second.Item2)
                return (first.Item1 + second.Item1) * first.Item2;
            //if (exp.IsProduct() || exp.IsSymbol())
            //{
            //    var common = FindCommonFactor(exp);
            //    if (common != null)
            //        return (this.ExcludeItem(common) + exp.ExcludeItem(common)) * common;
            //}
            //else if ((exp.IsSymbol() || exp.IsSum()) && IsEqualWithoutCoefficient(exp))
            //    return CoefficientIncrease();
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
        public Tuple<Number, Expression> SplitCoefficient()
        {
            Number coeff = (Number)Items.FirstOrDefault(n => n.IsNumber()) ?? new Number(1);
            return Tuple.Create<Number, Expression>(coeff, Extensions.CreateProduct(Items.Where(n => !n.IsNumber()).ToArray()));
        }
        public Number GetCoefficient()
        {
            return (Number)Items.FirstOrDefault(n => n.IsNumber()) ?? new Number(1);
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
