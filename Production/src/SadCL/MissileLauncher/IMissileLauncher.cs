using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    public interface IMissileLauncher
    {
        void fire();
        void moveTo(double x, double y);
        void moveBy(double x, double y);
        void reload();
        string status();

        string Name { get; set; }
        int CurrentMissiles { get; set; }
        int MaxMissiles { get; set; }
        double Phi { get; set; }

        double Theta { get; set; }
    }
}
