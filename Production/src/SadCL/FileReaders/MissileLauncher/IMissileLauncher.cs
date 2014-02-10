using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    interface IMissileLauncher
    {
        void fire();
        void moveTo(double x, double y);
        void moveBy(double x, double y);
        void reload();


    }
}
