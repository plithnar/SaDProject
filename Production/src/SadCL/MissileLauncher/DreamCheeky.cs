using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MissileLauncher
{
    public class DreamCheeky : IMissileLauncher
    {
	    private string[] nameList = {"Fry",
		    	     	     "Leela",
			         	     "Bender",
			         	     "Flexo",
			     	         "Professor",
			     	         "Lurr" };

        private int maxMissiles;
        private string name;
        private int currentMissiles;
        double currentPhi;
        double currentTheta;
        MissileLauncherControl controller;

        public DreamCheeky()
        {
            maxMissiles = 4;
            currentMissiles = maxMissiles;
            name = getNewName();
            controller = new MissileLauncherControl();
            controller.command_reset();
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
            currentMissiles = maxMissiles;
        }
        public void fire()
        {
            if (currentMissiles > 0)
            {
                controller.command_Fire();
                currentMissiles--;
            }
            else
                throw new InvalidOperationException();
        }

        public void moveBy(double phi, double theta)//double x, double y)
        {
            if(phi > 0)
            {
                controller.command_Right((int)Math.Floor(((phi) / 90) * 1600));
            }
            else
            {
                controller.command_Left((int)Math.Floor(((phi * -1.0) / 90) * 1600));
            }
            if(theta > 0)
            {
                controller.command_Up((int)Math.Floor(((theta) / 90) * 1600));
            }
            else
            {
                controller.command_Down((int)Math.Floor(((theta * -1.0) / 90) * 1600));
            }
            currentPhi += phi;
            currentTheta += theta;
        }

        public void moveTo(double phi, double theta)//double x, double y, double z)
        {
            //double radius = Math.Sqrt((x * x) + (y * y));
            //double phi = Math.Atan(y / x);
            //double theta = Math.Acos(z / radius);

            double phiOffset = phi - currentPhi;
            double thetaOffset = theta - currentTheta;

            if(phiOffset > 0)
            {
                controller.command_Right((int) Math.Floor(((phiOffset)/90) * 1600));
            }
            else
            {
                controller.command_Left((int)Math.Floor(((phiOffset * -1.0) / 90) * 1600));
            }

            if(thetaOffset > 0)
            {
                controller.command_Up((int)Math.Floor(((thetaOffset) / 90) * 1600));
            }
            else
            {
                controller.command_Down((int)Math.Floor(((thetaOffset * -1.0) / 90) * 1600));
            }
            currentTheta = theta;
            currentPhi = phi;
        }

        private string getNewName()
        {
            Random next = new Random();
            int namesIndex = next.Next(0, nameList.Length);
            
            return nameList[namesIndex];
        }

        public string status()
        {
            return ("Launcher: "+name+"\nMissiles: "+currentMissiles+" of "+maxMissiles+" remain");
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
