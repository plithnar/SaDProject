using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targets;

namespace Strategies
{
    public interface IStrategy
    {
        Target GetHighestPriorityTarget(IList<Target> targets, int gameTime);
    }
}
