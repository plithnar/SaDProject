using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targets;

namespace Strategies
{
    public class KillEmAllStrategy: IStrategy
    {
        private int index;

        public KillEmAllStrategy()
        {
            index = 0;
        }

        public Target GetHighestPriorityTarget(IList<Target> targets, int gameTime)
        {
            return targets[index++ % targets.Count];
        }
    }
}
