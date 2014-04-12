using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int MaxMissiles
        {
            get
            {
                return maxMissiles;
            }
            set
            {
                maxMissiles = value;
            }
        }

        public int CurrentMissiles
        {
            get
            {
                return currentMissiles;
            }
            set
            {
                currentMissiles = value;
            }
        }
        public double Phi
        {
            get
            {
                return currentPhi;
            }
            set
            {
                currentPhi = value;
            }
        }

        public double Theta
        {
            get
            {
                return currentTheta;
            }
            set
            {
                currentTheta = value;
            }
        }

        public Mock()
        {
            maxMissiles = 4;
            currentMissiles = maxMissiles;
            name = getNewName();
            Phi = 0.0;
            Theta = 0.0;
        }

        public void calibrate()
        {
            Phi = 0.0;
            Theta = 0.0;
        }

        public void reload()
        {
            Console.WriteLine("Reloading...");
            CurrentMissiles = maxMissiles;
        }
        public void fire()
        {
            if (currentMissiles > 0)
            {
                Console.WriteLine("Fire in the hole!");
                CurrentMissiles--;
            }
            else
                throw new InvalidOperationException();
        }

        public void moveBy(double phi, double theta)//double x, double y)
        {
            Console.WriteLine("Moving by "+phi+" "+theta);
            Phi += phi;
            Theta += theta;
        }

        public void moveTo(double phi, double theta)//double x, double y, double z)
        {
            Console.WriteLine("Moving by " + phi + " " + theta);
            double phiOffset = phi - currentPhi;
            double thetaOffset = theta - currentTheta;
            Theta = theta;
            Phi = phi;
        }

        private string getNewName()
        {
            Random next = new Random();
            int namesIndex = next.Next(0, nameList.Length);
            
            return nameList[namesIndex];
        }

        public string status()
        {
            return ("Launcher: " + name + "\nMissiles: " + currentMissiles + " of " + maxMissiles + " remain");
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
