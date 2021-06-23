using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public static class StepLogManager
    {
        public static Step CreateStep()
        {
            Step step = new Step();
            return step;
        }

        public static void Print(Step step, int depth = 0)
        {
            Console.WriteLine(new string(' ', depth) + step.Text);
            if (step.SubSteps != null)
            {
                foreach (Step sub in step.SubSteps)
                {
                    Print(sub, depth + 1);
                }
            }
        }
    }
}
