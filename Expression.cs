using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public abstract class Expression
    {
        public List<Expression> Items { get; set; }
        public abstract string Render();
        public abstract Expression Simplify();

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
        public void IfSingleAddRoot(Expression Root)
        {
            if (this.IsSingleItem() && this.Items.FirstOrDefault().IsNumber())
            {
                Root.Items.Remove(this);
                Root.Items.Add(this.Items.FirstOrDefault());
            }
        }

        public Expression()
        {
            Items = new List<Expression>();
        }
    }
}
