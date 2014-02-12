using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    public enum MissileLauncherTypes { DreamCheeky, Mock }
    public class MissileLauncherFactory
    {
        public static IMissileLauncher Create(MissileLauncherTypes launcherType)
        {
            IMissileLauncher launcher = null;
            switch (launcherType)
            {
                case MissileLauncherTypes.DreamCheeky:
                    launcher = new DreamCheeky();
                    break;
                case MissileLauncherTypes.Mock:
                    launcher = new Mock();
                    break;
            }
            return launcher;
        }
    }
}
