using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Targets
{
    public class TargetManager
    {
        private List<Target> m_targets;

        private static TargetManager m_instance;
        public List<Target> Friends
        {
            get
            {
                return (from i in m_targets
                        where i.Friend == true
                        select i).ToList();
            }
        }
        public List<Target> Enemies
        {
            get
            {
                return (from i in m_targets
                        where i.Friend == false
                        select i).ToList();
            }
        }

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
            Instance.m_targets = targets;
        }
    }
}
