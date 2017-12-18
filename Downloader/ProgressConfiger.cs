using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader
{
    public class ProgressConfiger
    {
        public event Action<int> onProgressStateChange;
        private int passed;
        private Stack<int> steps;

        public ProgressConfiger()
        {
            passed = 0;
            steps = new Stack<int>();
        }

        public void HandleCurrElem(int childrenCount)
        {
            int step = 100;
            //Якщо декілька нащадків
            if (childrenCount > 0)
            {
                if (steps.Count > 0)
                {
                    step = steps.Pop();
                }
                for(int i = 0; i < childrenCount; i++)
                {
                    steps.Push(step / childrenCount);
                }
            }
            else
            {
                //Якщо немає нащадків
                ProgressStateChange(steps.Pop());
            }
        }

        private int ProgressStateChange(int step)
        {
            passed += step;
            onProgressStateChange(passed);
            return passed;
        }
    }
}
