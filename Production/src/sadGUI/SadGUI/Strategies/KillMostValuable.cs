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
            bestScore = double.MinValue;
            for( int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                //CHECK FOR TIME STAMP
                if (target.HitCount > 0)
                {
                    if ((target.HitTime - gameTime) < (target.FlashRate * 10) /* MULTIPLIER IS FOR TESTING ONLY!!!*/)
                    {
                        continue;
                    }
                }

                //if alive at time of query
                int aliveFlag = 0;
                if(target.Alive)
                {
                    aliveFlag = 1;
                }
                int friendMultiplier;
                if(target.Friend)
                {
                    friendMultiplier = -1;
                }
                else
                {
                    friendMultiplier = 1;
                }
                if((target.Points * friendMultiplier * aliveFlag) > bestScore)
                {
                    bestScore = (target.Points * friendMultiplier * aliveFlag);
                    index = i;
                }
            }
            if (targets.Count > 0)
            {
                targets[index].HitTime = gameTime;
            }
            int shootIndex = index;
            index = 0;
            return targets[shootIndex];
        }
    }
}
