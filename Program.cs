using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression exp = Parser.Parse("a-(-5+6^i)");

            exp = exp.Simplify();
            string result = exp.Render();
            Console.WriteLine(result);
            Console.Read();
        }
    }
}
