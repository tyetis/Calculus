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
            string input = "(2*a)/(1*b)";
            Expression exp = Parser.Parse(input);

            exp = exp.Simplify();
            string result = exp.Render();
            Console.WriteLine($"Input  = {input}");
            Console.WriteLine($"Result = {result}");
            Console.WriteLine();
            Console.Read();
        }
    }
}
