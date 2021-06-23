using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class Expression
    {
        public bool IsPositive { get; set; }
        public List<Expression> Items { get; set; }
        public abstract string Render();
        public abstract Expression Simplify();
        public abstract bool IsEqual(Expression exp);

        protected abstract Expression Add(Expression exp);
        protected abstract Expression Multiply(Expression exp);
        public static Expression operator +(Expression exp1, Expression exp2)
        {
            return exp1.Add(exp2);
        }
        public static Expression operator *(Expression exp1, Expression exp2)
        {
            return exp1.Multiply(exp2);
        }
        public static bool operator ==(Expression exp1, Expression exp2)
        {
            return exp1.IsEqual(exp2);
        }
        public static bool operator !=(Expression exp1, Expression exp2)
        {
            if (exp1 is null && exp2 is null)
                return false;
            else if (exp2 is null) return true;
            return !exp1.IsEqual(exp2);
        }
        public void IfSingleAddRoot(Expression Root)
        {
            if (this.IsSingleItem() && this.Items.FirstOrDefault().IsNumber())
            {
                Root.Items.Remove(this);
                Root.Items.Add(this.Items.FirstOrDefault());
            }
        }
        public string DebuggerDisplay
        {
            get
            {
                return string.Format("{0} {1}", this.GetType(), Render());
            }
        }

        public Expression()
        {
            Items = new List<Expression>();
            IsPositive = true;
        }
    }
}
