using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculus
{
    public class Step
    {
        public List<Step> SubSteps { get; set; }
        public string Text { get; set; }
        public Step CreateSubStep()
        {
            Step sub = new Step();
            if (SubSteps == null) SubSteps = new List<Step>();
            SubSteps.Add(sub);
            return sub;
        }
    }
}
