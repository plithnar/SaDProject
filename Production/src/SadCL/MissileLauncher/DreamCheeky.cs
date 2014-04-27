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


        //Max Theta, phi offsets
        private double maxPhi = 90;
        private double minTheta = 0;
        private double maxTheta = 25;
        private int timeTo90 = 1500;
        public DreamCheeky()
        {
            maxMissiles = 4;
            currentMissiles = maxMissiles;
            name = getNewName();
            controller = new MissileLauncherControl();
            Phi = 0.0;
            Theta = 0.0;
        }

        public void calibrate()
        {
            controller.command_reset();
            moveBy(0.0, -5.0); // To correct for height over targets
            Phi = 0.0;
            Theta = 0.0;
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

        public void moveBy(double phi, double theta)
        {
            if(phi+currentPhi > maxPhi)
            {
                phi = maxPhi - currentPhi;
            }
            else if(phi+currentPhi < -maxPhi)
            {
                phi = -maxPhi - currentPhi;
            }

            if (theta + currentTheta > maxTheta)
            {
                theta = maxTheta - currentTheta;
            }
            else if (theta + currentTheta < minTheta)
            {
                theta = minTheta - currentTheta;
            }

            if(phi > 0)
            {
                controller.command_Right((int)Math.Floor(((phi) / maxPhi) * timeTo90));
            }
            else
            {
                controller.command_Left((int)Math.Floor(((phi * -1.0) / maxPhi) * timeTo90));
            }
            if(theta > 0)
            {
                controller.command_Up((int)Math.Floor(((theta) / maxPhi) * timeTo90));
            }
            else
            {
                controller.command_Down((int)Math.Floor(((theta * -1.0) / maxPhi) * timeTo90));
            }
            Phi += phi;
            Theta += theta;
            if(currentPhi > maxPhi)
            {
                Phi = maxPhi;
            }
            else if (currentPhi < -maxPhi)
            {
                Phi = -maxPhi;
            }
        }

        public void moveTo(double phi, double theta)//double x, double y, double z)
        {
            //double radius = Math.Sqrt((x * x) + (y * y));
            //double phi = Math.Atan(y / x);
            //double theta = Math.Acos(z / radius);

            if (phi > maxPhi)
            {
                phi = maxPhi;
            }
            if (phi < -maxPhi)
            {
                phi = -maxPhi;
            }

            if (theta > maxTheta)
            {
                theta = maxTheta;
            }
            if (theta < minTheta)
            {
                theta = minTheta;
            }
            double phiOffset = phi - currentPhi;
            double thetaOffset = theta - currentTheta;            

            if(phiOffset > 0)
            {
                controller.command_Right((int) Math.Floor(((phiOffset)/maxPhi) * timeTo90));
            }
            else
            {
                controller.command_Left((int)Math.Floor(((phiOffset * -1.0) / maxPhi) * timeTo90));
            }

            if(thetaOffset > 0)
            {
                controller.command_Up((int)Math.Floor(((thetaOffset) / maxPhi) * timeTo90));
            }
            else
            {
                controller.command_Down((int)Math.Floor(((thetaOffset * -1.0) / maxPhi) * timeTo90));
            }
            currentTheta = theta;
            currentPhi = phi;
            if (currentPhi > maxPhi)
            {
                currentPhi = maxPhi;
            }
            else if (currentPhi < -maxPhi)
            {
                currentPhi = -maxPhi;
            }
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
