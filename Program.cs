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
            //string input = "-(-a*b)";
            //string input = "-a*b*(-1)";
            string input = "2*a*b-a";
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
