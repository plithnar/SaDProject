using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetReader
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

        // Constructor
        public Target(string name, double x, double y, double z,
                     bool friend, int points, int flashrate)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
            Friend = friend;
            Points = points;
            FlashRate = flashrate;
        }
    }
    public class TargetManager
    {
        private List<Target> m_targets;

        private static TargetManager m_instance;

        // Property for instance of singleton class
        public static TargetManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new TargetManager();
                }
                return m_instance;
            }
        }

        // Private singleton constructor 
        private TargetManager()
        {
            m_targets = new List<Target>();
        }

        /*
         * Returns list of all target names
         */
        public List<string> GetNames()
        {
            List<string> names = new List<string>();

            foreach (var i in m_targets)
            {
                names.Add(i.Name);
            }

            return names;
        }

        /* 
         * Returns true if a target with a particular name is Friendly
         * returns false otherwise
         */
        public bool IsFriendly(string name)
        {
            return (from i in m_targets
                             where i.Name.ToLower() == name
                             select i).First().Friend;
        }
        /*
         * Return target given target name
         */
        public Target GetTarget(string name)
        {
            return (from i in m_targets
                             where i.Name.ToLower() == name
                             select i).First();
        }
        /*
         * Add a target to m_targets.
         */
        public static void AddTarget(Target target)
        {
            Instance.m_targets.Add(target);
        }
        /*
         * Add List of targets to the end m_targets list
         */
        public static void AddTargets(List<Target> targets)
        {
            Instance.m_targets = Instance.m_targets.Concat(targets).ToList<Target>();
        }
    }
}
