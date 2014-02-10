using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbLibrary;
using MissileLauncher;

namespace MissileLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            MissileLauncherControl controller = new MissileLauncherControl();
            DreamCheeky missile = new DreamCheeky();

            missile.moveTo(45, 10);
            missile.fire();
            //missile.fire();
            missile.fire();
            //missile.fire();

            missile.printStatus();
            
            missile.moveBy(-20, 10);
            //missile.reload();
            controller.command_reset();
        }
    }
}
