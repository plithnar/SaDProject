using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    public class Conversions
    {
        public static double radiansToDegrees(double angle)
        {
            return ((angle * 180) / Math.PI);
        }
        public static double calcRadius(double x, double y, double z)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }
        public static double calcPhi(double x, double y, double z)
        {
            return radiansToDegrees(Math.Acos(z / calcRadius(x, y, z)));
        }
        public static double calcTheta(double x, double y)
        {
            return radiansToDegrees(Math.Atan(y / x));
        }
    }
}
