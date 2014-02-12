using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    public class Mock : IMissileLauncher
    {
	private string[] nameList = {"MockFry",
			     	     "MockLeela",
			     	     "MockBender",
			     	     "MockFlexo",
			     	     "MockProfessor",
			     	     "MockLurr" };

        private int maxMissiles;
        private string name;
        private int currentMissiles;
        double currentPhi;
        double currentTheta;

        public Mock()
        {
            maxMissiles = 4;
            currentMissiles = maxMissiles;
            name = getNewName();
            currentPhi = 0.0;
            currentTheta = 0.0;
        }

        public int MaxMissles
        {
            get { return MaxMissles; }
            private set { MaxMissles = value; }
        }

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public void reload()
        {
            Console.WriteLine("Reloading...");
            currentMissiles = maxMissiles;
        }
        public void fire()
        {
            Console.WriteLine("Fire in the hole!");
            currentMissiles--;
        }

        public void moveBy(double phi, double theta)//double x, double y)
        {
            Console.WriteLine("Moving by "+phi+" "+theta);
            currentPhi += phi;
            currentTheta += theta;
        }

        public void moveTo(double phi, double theta)//double x, double y, double z)
        {
            Console.WriteLine("Moving by " + phi + " " + theta);
            double phiOffset = phi - currentPhi;
            double thetaOffset = theta - currentTheta;
            currentTheta = theta;
            currentPhi = phi;
        }

        private string getNewName()
        {
            Random next = new Random();
            int namesIndex = next.Next(0, nameList.Length);
            
            return nameList[namesIndex];
        }

        public void printStatus()
        {
            Console.WriteLine("Launcher: {0} \nMissiles: {1} of {2} remain", name, currentMissiles, maxMissiles);
        }

        enum POSSIBLE_NAMES
        {
            Fry,
            Leela,
            Bender,
            Flexo,
            Professor,
            Lurr
        }
    }
}
