using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targets;


namespace Strategies
{
    public class KillMostValuable : IStrategy
    {
        private int index;
        private double bestScore;

        public KillMostValuable()
        {
            index = 0;
            bestScore = double.MinValue;
        }

        public Target GetHighestPriorityTarget(IList<Target> targets, int gameTime)
        {
            for(int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                //CHECK FOR TIME STAMP
                
                if((target.HitTime - gameTime) < (target.FlashRate * 100) /* MULTIPLIER IS FOR TESTING ONLY!!!*/)
                {
                    continue;
                }

                //if alive at time of query
                int friendMultiplier;
                if(target.Friend)
                {
                    friendMultiplier = -1;
                }
                else
                {
                    friendMultiplier = 1;
                }
                if(target.Points * friendMultiplier > bestScore)
                {
                    index = i;
                }
            }
            targets[index].HitTime = gameTime;
            int shootIndex = index;
            index = 0;
            return targets[shootIndex];
        }
    }
}
