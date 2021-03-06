﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Targets
{
    public class Target
    {
        // Properties
        public string Name { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public bool Friend { get; private set; }
        public int Points { get; private set; }
        public int FlashRate { get; private set; }
        public bool Alive { get; set; }
        public double SpawnRate { get; private set; }
        public int HitTime { get; set; }

        public int HitCount { get; set; }

        public void Kill()
        {
            Alive = false;
        }

        // Constructor
        public Target(string name, double x, double y, double z,
                     bool friend, int points, int flashrate, int hitTime=int.MaxValue, int hitCount=0)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
            Friend = friend;
            Points = points;
            FlashRate = flashrate;
            Alive = true;
            HitTime = hitTime;
            HitCount = hitCount;
        }
    }
}
